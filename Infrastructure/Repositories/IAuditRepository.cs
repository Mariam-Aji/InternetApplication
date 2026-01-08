using WebAPI.Domain.Entities;

public interface IAuditRepository
{
    Task AddLogAsync(AuditLog log);
    Task<IEnumerable<AuditLog>> GetAllLogsAsync();
}