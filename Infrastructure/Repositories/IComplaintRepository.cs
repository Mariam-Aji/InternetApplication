using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintRepository
    {
        Task AddAsync(Complaint complaint);
    }
}
