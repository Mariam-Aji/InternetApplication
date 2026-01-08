using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

namespace  WebAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) { _db = db; }

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(Guid id)
        => await _db.Users.FindAsync(id);

    public async Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }
    public  async Task SeedAdminAsync()
    {
        var adminExists = await _db.Users.AnyAsync(u => u.Role == "Admin");

        if (!adminExists)
        {
            var adminUser = new User
            {
                FullName = "System Admin",
                Email = "admin@gmail.com",
                IsEmailConfirmed=true,
                Role = "Admin",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "Admin@123")
            };

             _db.Users.Add(adminUser);
            await _db.SaveChangesAsync();


        }


    }
    public async Task<User?> GetByIdIntAsync(int id)
    => await _db.Users.FindAsync(id);

    public async Task UpdateUserAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _db.Users
            .Where(u => u.Id != 1)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var histories = await _db.ComplaintHistories
                .Where(h => h.EmployeeId == user.Id)
                .ToListAsync();
            if (histories.Any()) _db.ComplaintHistories.RemoveRange(histories);

            var complaints = await _db.Complaints
                .Where(c => c.UserId == user.Id)
                .ToListAsync();
            if (complaints.Any()) _db.Complaints.RemoveRange(complaints);

            var otps = await _db.OtpCodes
                .Where(o => o.UserId == user.Id)
                .ToListAsync();
            if (otps.Any()) _db.OtpCodes.RemoveRange(otps);

            _db.Users.Remove(user);

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
           
            await transaction.RollbackAsync();
            return false;
        }
    }
    public async Task<Dictionary<string, int>> GetUsersCountByRoleAsync()
    {
        var counts = await _db.Users
            .GroupBy(u => u.Role.ToUpper())
            .Select(g => new { Role = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Role, x => x.Count);

        return counts;
    }
}
