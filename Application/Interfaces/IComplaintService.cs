using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintService
    {
        Task AddComplaintAsync(ComplaintRequest request);

    }
}
