using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IGovernmentAgencyService
    {
        Task<(bool Success, string Message)> AddAgencyAsync(GovernmentAgency agency);

        Task<List<GovernmentAgency>> GetAllAgenciesAsync();

    }
}
