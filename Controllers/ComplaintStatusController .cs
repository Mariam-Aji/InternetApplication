using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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


        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _service.GetAllStatusesAsync();
            return Ok(statuses);
        }
        [Authorize(Roles = "Citizen")]
        [HttpGet("my-complaints")]
        public async Task<IActionResult> GetMyComplaints()
        {
            var citizenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (citizenId == null)
            {
                return Unauthorized(new
                {
                    status = 401,
                    message = "unauthorize"
                });
            }

            int userId = int.Parse(citizenId);

            var complaints = await _service.GetUserComplaintsAsync(userId);

            return Ok(new
            {
                status = 200,
                total = complaints.Count,
                complaints = complaints.Select(c => new
                {
                    id = c.Id,
                    Date=c.ComplaintDate,
                    governmentAgencyName = c.GovernmentAgency.AgencyName,  
                    description=c.Description,
                    statusName = c.ComplaintStatus.StatusName         
                })
            });
        }

    }
}
