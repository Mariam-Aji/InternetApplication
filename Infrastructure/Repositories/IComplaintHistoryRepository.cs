namespace WebAPI.Infrastructure.Repositories
{
    public interface IComplaintHistoryRepository
    {
        Task AddHistoryAsync(int complaintId, int employeeId, string actionType, string? newValue);
    }
}
