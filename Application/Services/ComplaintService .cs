using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Hubs;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI.Application.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _repo;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IComplaintHistoryRepository _historyRepo;
        public ComplaintService(
        IComplaintRepository repo,
        IComplaintHistoryRepository historyRepo,
        IHubContext<NotificationHub> hub)
        {
            _repo = repo;
            _historyRepo = historyRepo; 
            _hub = hub;
        }

        public async Task<Complaint?> AddComplaintAsync(ComplaintRequest request)
        {
            var exists = await _repo.GovernmentAgencyExistsAsync(request.GovernmentAgencyId);
            if (!exists) return null;

            var complaint = new Complaint
            {
                ComplaintType = request.ComplaintType,
                Location = request.Location,
                Description = request.Description,
                UserId = request.UserId,
                GovernmentAgencyId = request.GovernmentAgencyId,
                ComplaintStatusId = 1,
                ComplaintDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var imagePaths = new List<string>();

            if (request.Images != null && request.Images.Count > 0)
            {
                foreach (var image in request.Images)
                {
                    var savedPath = await SaveFileAsync(image);
                    imagePaths.Add(savedPath);
                }
            }

            complaint.Images = JsonSerializer.Serialize(imagePaths);

            complaint.PdfFile = await SaveFileAsync(request.PdfFile);

            await _repo.AddAsync(complaint);

            await _hub.Clients.Group($"Citizen_{complaint.UserId}")
                .SendAsync("ReceiveNotification", new
                {
                    complaintId = complaint.Id,
                    message = "تم التسليم بنجاح"
                });

            return complaint;
        }


        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine("Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return filePath;
        }
        public async Task<bool> UpdateComplaintFilesAsync(int complaintId, int userId, UpdateComplaintFilesDto dto)
        {
            var complaint = await _repo.GetByIdAsync(complaintId);
            if (complaint == null || complaint.UserId != userId) return false;


            List<string> changes = new List<string>();


            if (dto.Images != null && dto.Images.Any())
            {
                var imageRelativePaths = new List<string>();
                foreach (var file in dto.Images)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var relativePath = Path.Combine("Uploads", fileName);

                    using (var stream = new FileStream(relativePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    imageRelativePaths.Add(relativePath);
                }
                complaint.Images = JsonSerializer.Serialize(imageRelativePaths);
                changes.Add("تحديث الصور المرفقة");
            }


            if (dto.PdfFile != null)
            {
                var pdfFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PdfFile.FileName)}";
                var pdfRelativePath = Path.Combine("Uploads", pdfFileName);

                using (var stream = new FileStream(pdfRelativePath, FileMode.Create))
                {
                    await dto.PdfFile.CopyToAsync(stream);
                }
                complaint.PdfFile = pdfRelativePath;
                changes.Add("تحديث ملف PDF");
            }


            var isUpdated = await _repo.UpdateAsync(complaint);


            if (isUpdated)
            {
                var history = new ComplaintHistory
                {
                    ComplaintId = complaintId,
                    EmployeeId = userId,
                    ActionType = "AttachmentUpdated",
                    NewValue = string.Join(" و ", changes),
                    ActionDate = DateTime.Now
                };


                await _repo.AddHistoryAsync(history);
            }

            return isUpdated;
        }


        public async Task<ComplaintStatisticsDto> GetComplaintsStatisticsAsync()
        {
            var counts = await _repo.GetComplaintsCountByStatusAsync();

            return new ComplaintStatisticsDto
            {
                TotalComplaints = counts.Values.Sum(),

                NewComplaints = counts.GetValueOrDefault(1), // جديدة
                UnderProcess = counts.GetValueOrDefault(2), // قيد المعالجة
                Completed = counts.GetValueOrDefault(3),    // منجزة
                Rejected = counts.GetValueOrDefault(4)     // مرفوضة
            };
        }
      
        public async Task<IEnumerable<ComplaintHistoryDto>> GetHistoryByComplaintIdAsync(int complaintId)
        {
            var histories = await _historyRepo.GetComplaintHistoriesAsync(complaintId);

            return histories.Select(h => new ComplaintHistoryDto
            {
                Id = h.Id,
                ComplaintId = h.ComplaintId,
                ActionType = h.ActionType,
                NewValue = h.NewValue,
                ActionDate = h.ActionDate,
                PerformedBy = h.Employee != null ? h.Employee.FullName : "System / Unknown"
            });
        }

        public async Task<IEnumerable<ComplaintHistoryDto>> GetAllComplaintsHistoryAsync()
        {
            var histories = await _historyRepo.GetAllHistoriesAsync();

            return histories.Select(h => new ComplaintHistoryDto
            {
                Id = h.Id,
                ComplaintId = h.ComplaintId,
                ActionType = h.ActionType,
                NewValue = h.NewValue,
                ActionDate = h.ActionDate,
                PerformedBy = h.Employee != null ? h.Employee.FullName : "نظام آلي"
            });
        }
        public async Task<PerformanceMetricsDto> GetSystemPerformanceAsync()
        {
            var metrics = await _repo.GetPerformanceMetricsAsync();

            return metrics;
        }
        public async Task<List<Complaint>> GetAllComplaintsAsync( )
        {
            return await _repo.GetALLComplaintsAsync();
        }
    }
}
