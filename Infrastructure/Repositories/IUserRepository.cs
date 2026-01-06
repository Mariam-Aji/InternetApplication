using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<User?> GetByIdIntAsync(int id);
    Task UpdateUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(User user);
    Task<Dictionary<string, int>> GetUsersCountByRoleAsync();
}
