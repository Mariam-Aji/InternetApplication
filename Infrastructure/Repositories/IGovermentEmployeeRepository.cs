using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Repositories
{
    public interface IGovermentEmployeeRepositry
    {
        Task<List<Complaint>> GetComplaintsForEmployeeAsync(int userId);
        Task<bool> LockComplaintAsync(int complaintId, int employeeId);
         Task UnlockComplaintAsync(int complaintId);
        Task<(bool Success, int? CitizenId, string StatusName)> UpdateStatusAsync(int complaintId, int newStatusId);
        Task<(bool Success, int? CitizenId)> AddNoteAsync(int complaintId, string note);
    }
}
