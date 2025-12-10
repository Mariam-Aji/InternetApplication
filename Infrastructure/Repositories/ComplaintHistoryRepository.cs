using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;


namespace WebAPI.Infrastructure.Repositories
{
    public class ComplaintHistoryRepository : IComplaintHistoryRepository
    {
        private readonly AppDbContext _context;

        public ComplaintHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddHistoryAsync(int complaintId, int employeeId, string actionType, string? newValue)
        {
            var history = new ComplaintHistory
            {
                ComplaintId = complaintId,
                EmployeeId = employeeId,
                ActionType = actionType,
                
                NewValue = newValue,
                ActionDate = DateTime.Now
            };

            _context.ComplaintHistories.Add(history);
            await _context.SaveChangesAsync();
        }
    }

}
