using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IGovernmentAgencyRepository
    {
        Task AddAsync(GovernmentAgency agency);
        Task<GovernmentAgency?> GetByNameAsync(string name);
        Task<List<GovernmentAgency>> GetAllAsync();
    }
}
