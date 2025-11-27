using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IGovermentEmployeeService
    {
        Task<List<Complaint>> GetEmployeeComplaintsAsync(int userId);
        Task<bool> LockComplaintAsync(int employeeId, int complaintId);
         Task UnlockComplaintAsync(int complaintId);
        Task<(bool Success, int? CitizenId, string StatusName)> UpdateStatusAsync(int complaintId, int newStatusId);
        Task<(bool Success, int? CitizenId)> AddNoteAsync(int complaintId, string note);
    }
}
