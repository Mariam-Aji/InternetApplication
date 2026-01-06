using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintStatusService
    {
      
        Task<List<ComplaintStatus>> GetAllStatusesAsync();
        Task<List<Complaint>> GetUserComplaintsAsync(int userId);

    }
}
