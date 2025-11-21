using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

namespace WebAPI.Infrastructure.Repositories
{
    public class ComplaintStatusRepository : IComplaintStatusRepository
    {
        private readonly AppDbContext _db;

        public ComplaintStatusRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ComplaintStatus status)
        {
            _db.ComplaintStatuses.Add(status);
            await _db.SaveChangesAsync();
        }

        public async Task<ComplaintStatus?> GetByNameAsync(string name)
        {
            return await _db.ComplaintStatuses
                .FirstOrDefaultAsync(x => x.StatusName == name);
        }

        public async Task<List<ComplaintStatus>> GetAllAsync()
        {
            return await _db.ComplaintStatuses.ToListAsync();
        }
    }
}
