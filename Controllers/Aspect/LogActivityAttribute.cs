using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Security.Claims;
using WebAPI.Domain.Entities;

public class LogActivityAttribute : IAsyncActionFilter
{
    private readonly IAuditService _auditService;
    public LogActivityAttribute(IAuditService auditService) { _auditService = auditService; }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        var executedContext = await next();

        stopwatch.Stop();

        var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                  ?? context.HttpContext.User.FindFirst("sub")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = context.RouteData.Values["action"]?.ToString(),
                Controller = context.RouteData.Values["controller"]?.ToString(),
                Timestamp = DateTime.Now, 
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                IpAddress = "System" 
            };

            await _auditService.RecordActivityAsync(log);
        }
    }
}