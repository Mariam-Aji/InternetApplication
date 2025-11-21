using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintStatusController : ControllerBase
    {
        private readonly IComplaintStatusService _service;

        public ComplaintStatusController(IComplaintStatusService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddStatus([FromBody] ComplaintStatus status)
        {
            var exists = await _service.GetAllStatusesAsync();
            if (exists.Any(s => s.StatusName == status.StatusName))
                return BadRequest(new { Message = "هذه الحالة موجودة مسبقًا" });

            await _service.AddStatusAsync(status);

            return StatusCode(201, new
            {
                Message = "تمت إضافة الحالة بنجاح",
                StatusId = status.Id
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _service.GetAllStatusesAsync();
            return Ok(statuses);
        }
    }
}
