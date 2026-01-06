using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

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
                ReportGeneratedAt = DateTime.Now
            };
        }
    } }
