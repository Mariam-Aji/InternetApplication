using WebAPI.Domain.Entities;

public interface IAuditService
{
    Task RecordActivityAsync(AuditLog log);
}