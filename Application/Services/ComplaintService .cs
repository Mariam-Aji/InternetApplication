using Microsoft.AspNetCore.SignalR;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Hubs;

namespace WebAPI.Application.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _repo;
        private readonly IHubContext<NotificationHub> _hub;

        public ComplaintService(IComplaintRepository repo, IHubContext<NotificationHub> hub)
        {
            _repo = repo;
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
                ComplaintStatusId = 1
            };

            complaint.Image1 = await SaveFileAsync(request.Image1);
            complaint.Image2 = request.Image2 != null ? await SaveFileAsync(request.Image2) : null;
            complaint.Image3 = request.Image3 != null ? await SaveFileAsync(request.Image3) : null;
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
    }
}
