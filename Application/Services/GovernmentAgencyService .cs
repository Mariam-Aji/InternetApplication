using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Services
{
    public class GovernmentAgencyService : IGovernmentAgencyService
    {
        private readonly IGovernmentAgencyRepository _repo;

        public GovernmentAgencyService(IGovernmentAgencyRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string Message)> AddAgencyAsync(GovernmentAgency agency)
        {
            var exists = await _repo.GetByNameAsync(agency.AgencyName);

            if (exists != null)
                return (false, "هذه الجهة موجودة مسبقًا");

            await _repo.AddAsync(agency);

            return (true, "تمت إضافة الجهة بنجاح");
        }

        public async Task<List<GovernmentAgency>> GetAllAgenciesAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
