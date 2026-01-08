using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

public class AuditRepository : IAuditRepository
{
    private readonly AppDbContext _context;
    public AuditRepository(AppDbContext context) { _context = context; }

    public async Task AddLogAsync(AuditLog log)
    {
        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetAllLogsAsync()
    {
        
        return await _context.AuditLogs
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
}