using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintStatusService
    {
        Task AddStatusAsync(ComplaintStatus status);
        Task<List<ComplaintStatus>> GetAllStatusesAsync();
    }
}
