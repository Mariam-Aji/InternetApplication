using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;
using WebAPI.Infrastructure.Db;

namespace WebAPI.Application.Services
{
    public class ComplaintReportService : IComplaintReportService
    {
        private readonly AppDbContext _context;

        public ComplaintReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DailyComplaintReportDto>> GetTodayComplaintsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var complaints = await _context.Complaints
           .Include(c => c.User)
                 .Include(c => c.ComplaintStatus)
           .Include(c => c.Histories)
              .Where(c => c.ComplaintDate == today)
                .Select(c => new
           {
             c.Id,
             c.ComplaintType,
             c.Location,
               Status = c.ComplaintStatus.StatusName,
            UserName = c.User.FullName,
             c.ComplaintDate,
              c.Images,
             Histories = c.Histories
         })
         .ToListAsync();

           

            return complaints.Select(c => new DailyComplaintReportDto
            {
                Id = c.Id,
                Type = c.ComplaintType,
                Location = c.Location,
                Status = c.Status,
                UserName = c.UserName,
                Date = c.ComplaintDate,
                ImagePaths = string.IsNullOrWhiteSpace(c.Images)
        ? new List<string>()
        : JsonSerializer.Deserialize<List<string>>(c.Images) ?? new List<string>(),
                History = c.Histories.Select(h => new ComplaintHistoryReportDto
                {
                    ActionType = h.ActionType,
                    NewValue = h.NewValue,
                    ActionDate = h.ActionDate
                }).ToList()
            }).ToList();
        }
        public async Task<List<DailyComplaintReportDto>> GetLast7DaysComplaintsAsync()
        {
            var to = DateOnly.FromDateTime(DateTime.Now);
            var from = to.AddDays(-7);

            var complaints = await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.ComplaintStatus)
                .Include(c => c.Histories)
                .Where(c => c.ComplaintDate >= from && c.ComplaintDate <= to)
                .ToListAsync();

            return complaints.Select(c => new DailyComplaintReportDto
            {
                Id = c.Id,
                Type = c.ComplaintType,
                Location = c.Location,
                Status = c.ComplaintStatus?.StatusName,
                UserName = c.User?.FullName,
                Date = c.ComplaintDate,
                ImagePaths = string.IsNullOrWhiteSpace(c.Images)
                    ? new()
                    : JsonSerializer.Deserialize<List<string>>(c.Images) ?? new(),
                History = c.Histories?.Select(h => new ComplaintHistoryReportDto
                {
                    ActionType = h.ActionType,
                    NewValue = h.NewValue,
                    ActionDate = h.ActionDate
                }).ToList() ?? new()
            }).ToList();
        }

    }

}
