//using WebAPI.Migrations;

namespace WebAPI.Infrastructure.Repositories
{
    public interface IComplaintHistoryRepository
    {
        Task AddHistoryAsync(int complaintId, int employeeId, string actionType, string? newValue);
        Task<IEnumerable<WebAPI.Domain.Entities.ComplaintHistory>> GetComplaintHistoriesAsync(int complaintId);

        Task<IEnumerable<WebAPI.Domain.Entities.ComplaintHistory>> GetAllHistoriesAsync();

    }
}
