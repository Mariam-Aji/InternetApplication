using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] 
public class AuditController : ControllerBase
{
    private readonly IAuditRepository _auditRepo;
    public AuditController(IAuditRepository auditRepo) { _auditRepo = auditRepo; }

    [HttpGet("all-logs")]
    public async Task<IActionResult> GetAllAuditLogs()
    {
        var logs = await _auditRepo.GetAllLogsAsync();
        return Ok(new { status = 200, data = logs });
    }
}