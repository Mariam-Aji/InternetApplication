using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintStatusRepository
    {
        Task AddAsync(ComplaintStatus status);
        Task<ComplaintStatus?> GetByNameAsync(string name);
        Task<List<ComplaintStatus>> GetAllAsync();
    }
}
