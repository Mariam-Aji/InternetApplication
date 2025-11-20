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
    }
