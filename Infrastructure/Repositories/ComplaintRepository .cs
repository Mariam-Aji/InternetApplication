using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

namespace WebAPI.Infrastructure.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _db;

        public ComplaintRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Complaint complaint)
        {
            _db.Complaints.Add(complaint);
            await _db.SaveChangesAsync();
        }
    }
}
