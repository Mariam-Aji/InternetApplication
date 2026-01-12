using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

using System.Diagnostics;
using WebAPI.Application.DTOs;

namespace WebAPI.Infrastructure.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _db;

        public ComplaintRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Complaint complaint)
        {
            _db.Complaints.Add(complaint);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> GovernmentAgencyExistsAsync(int id)
        {
            return await _db.GovernmentAgencies.AnyAsync(a => a.Id == id);
        }

        public async Task<string?> GetComplaintStatusNameAsync(int statusId)
        {
            return await _db.ComplaintStatuses
                            .Where(s => s.Id == statusId)
                            .Select(s => s.StatusName)
                            .FirstOrDefaultAsync();
        }
        public async Task<int?> GetCitizenIdByComplaintIdAsync(int complaintId)
        {
            return await _db.Complaints
                .Where(c => c.Id == complaintId)
                .Select(c => c.UserId)
                .FirstOrDefaultAsync();
        }
        public async Task<Complaint> GetByIdAsync(int id)
        {
            return await _db.Complaints.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Complaint complaint)
        {
            _db.Complaints.Update(complaint);
            return await _db.SaveChangesAsync() > 0;
        }


        public async Task AddHistoryAsync(ComplaintHistory history)
        {
            await _db.ComplaintHistories.AddAsync(history);
            await _db.SaveChangesAsync();
        }
<<<<<<< HEAD
    
    public async Task<Dictionary<int, int>> GetComplaintsCountByStatusAsync()
=======

        public async Task<Dictionary<int, int>> GetComplaintsCountByStatusAsync()
>>>>>>> f8b3d41 (Performance: optimize server to handle up to 100 concurrent users)
        {
            return await _db.Complaints
                .GroupBy(c => c.ComplaintStatusId ?? 1)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.StatusId, x => x.Count);
        }
        public async Task<IEnumerable<ComplaintHistory>> GetComplaintHistoriesAsync(int complaintId)
        {
            return await _db.ComplaintHistories
<<<<<<< HEAD
                .Include(h => h.Employee) 
=======
                .Include(h => h.Employee)
>>>>>>> f8b3d41 (Performance: optimize server to handle up to 100 concurrent users)
                .Where(h => h.ComplaintId == complaintId)
                .OrderByDescending(h => h.ActionDate)
                .ToListAsync();
        }



<<<<<<< HEAD
 
        public async Task<PerformanceMetricsDto> GetPerformanceMetricsAsync()
        {
            var total = await _db.Complaints.CountAsync();
            var completedCount = await _db.Complaints.CountAsync(c => c.ComplaintStatusId == 3);

            double avgPendingDays = 0;
            var pendingComplaints = await _db.Complaints
                .Where(c => c.ComplaintStatusId != 3 && c.ComplaintDate != null) 
                .ToListAsync();

            if (pendingComplaints.Any())
            {
                DateTime today = DateTime.Today;

                avgPendingDays = pendingComplaints.Average(c =>
                {
                   
                    DateTime complaintDateTime = c.ComplaintDate!.Value.ToDateTime(TimeOnly.MinValue);
                    return (today - complaintDateTime).TotalDays;
                });
            }

            return new PerformanceMetricsDto
            {
                TotalComplaints = total,
                CompletionRate = total > 0 ? (double)completedCount / total * 100 : 0,
                PendingComplaints = await _db.Complaints.CountAsync(c => c.ComplaintStatusId == 1),
                AverageProcessingTimeDays = Math.Round(avgPendingDays, 2),
                SystemMemoryUsageBytes = GC.GetTotalMemory(false),
=======

        public async Task<PerformanceMetricsDto> GetPerformanceMetricsAsync()
        {
            var currentProcess = Process.GetCurrentProcess();
            double memoryUsageInMB = currentProcess.WorkingSet64 / (1024.0 * 1024.0);


            var startTime = DateTime.UtcNow;
            var startCpuUsage = currentProcess.TotalProcessorTime;
            await Task.Delay(100);
            currentProcess.Refresh();
            var endCpuUsage = currentProcess.TotalProcessorTime;
            var endTime = DateTime.UtcNow;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsagePercent = (cpuUsedMs / (Environment.ProcessorCount * totalMsPassed)) * 100;

            return new PerformanceMetricsDto
            {
                MemoryUsageMB = $"{Math.Round(memoryUsageInMB, 2)} MB",
                CpuUsagePercentage = $"{Math.Round(cpuUsagePercent, 2)}%",
>>>>>>> f8b3d41 (Performance: optimize server to handle up to 100 concurrent users)
                ReportGeneratedAt = DateTime.Now
            };
        }
        public async Task<List<Complaint>> GetALLComplaintsAsync()
        {
<<<<<<< HEAD
            
=======

>>>>>>> f8b3d41 (Performance: optimize server to handle up to 100 concurrent users)
            return await _db.Complaints
                .Include(c => c.User)
                .Include(c => c.ComplaintStatus)
                .Include(c => c.GovernmentAgency)

<<<<<<< HEAD
                .ToListAsync() ?? new List<Complaint>(); 
        }
    }
} 

         
=======
                .ToListAsync() ?? new List<Complaint>();
        }
    }
}


>>>>>>> f8b3d41 (Performance: optimize server to handle up to 100 concurrent users)

