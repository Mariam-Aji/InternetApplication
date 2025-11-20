using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

namespace WebAPI.Infrastructure.Repositories
{
    public class GovernmentAgencyRepository : IGovernmentAgencyRepository
    {
        private readonly AppDbContext _db;

        public GovernmentAgencyRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(GovernmentAgency agency)
        {
            _db.GovernmentAgencies.Add(agency);
            await _db.SaveChangesAsync();
        }

        public async Task<GovernmentAgency?> GetByNameAsync(string name)
        {
            return await _db.GovernmentAgencies
                .FirstOrDefaultAsync(x => x.AgencyName == name);
        }

        public async Task<List<GovernmentAgency>> GetAllAsync()
        {
            return await _db.GovernmentAgencies.ToListAsync();
        }
    }
}
