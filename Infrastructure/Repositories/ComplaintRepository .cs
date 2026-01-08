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
    
    public async Task<Dictionary<int, int>> GetComplaintsCountByStatusAsync()
        {
            return await _db.Complaints
                .GroupBy(c => c.ComplaintStatusId ?? 1)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.StatusId, x => x.Count);
        }
        public async Task<IEnumerable<ComplaintHistory>> GetComplaintHistoriesAsync(int complaintId)
        {
            return await _db.ComplaintHistories
                .Include(h => h.Employee) 
                .Where(h => h.ComplaintId == complaintId)
                .OrderByDescending(h => h.ActionDate)
                .ToListAsync();
        }


        public async Task<PerformanceMetricsDto> GetPerformanceMetricsAsync()
        {
            var currentProcess = Process.GetCurrentProcess();

            double memoryInMB = currentProcess.WorkingSet64 / (1024.0 * 1024.0);

            var startTime = DateTime.UtcNow;
            var startCpuUsage = currentProcess.TotalProcessorTime;

            await Task.Delay(200);

            currentProcess.Refresh();
            var endCpuUsage = currentProcess.TotalProcessorTime;
            var endTime = DateTime.UtcNow;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            var cpuUsagePercent = totalMsPassed > 0
                ? (cpuUsedMs / (Environment.ProcessorCount * totalMsPassed)) * 100
                : 0;

            return new PerformanceMetricsDto
            {
                MemoryUsageMB = $"{Math.Round(memoryInMB, 2)} MB",
                CpuUsagePercentage = $"{Math.Round(cpuUsagePercent, 2)}%",
                ActiveThreads = currentProcess.Threads.Count,
                ReportGeneratedAt = DateTime.Now
            };
        }

        public async Task<List<Complaint>> GetALLComplaintsAsync()
        {
            return await _db.Complaints
                .Include(c => c.User)
                .Include(c => c.ComplaintStatus)
                .Include(c => c.GovernmentAgency)
                .ToListAsync() ?? new List<Complaint>(); 
        }
    }
} 
