using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

public class AuditService : IAuditService
{
    private readonly IAuditRepository _repo;
    private readonly AppDbContext _context;

    public AuditService(IAuditRepository repo, AppDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task RecordActivityAsync(AuditLog log)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id.ToString() == log.UserId);

        log.UserName = user?.FullName ?? $"User {log.UserId}";

        await _repo.AddLogAsync(log);
    }
}