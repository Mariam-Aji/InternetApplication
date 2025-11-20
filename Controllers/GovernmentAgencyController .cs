using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GovernmentAgencyController : ControllerBase
    {
        private readonly IGovernmentAgencyService _service;

        public GovernmentAgencyController(IGovernmentAgencyService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddAgency([FromForm] GovernmentAgency agency)
        {
            var result = await _service.AddAgencyAsync(agency);

            if (!result.Success)
                return Conflict(new { Message = result.Message }); 

            return StatusCode(201, new
            {
                Message = result.Message,
                AgencyId = agency.Id
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAgencies()
        {
            try
            {
                var agencies = await _service.GetAllAgenciesAsync();

                return Ok(new
                {
                    Message = "تم عرض البيانات بنجاح",
                    Agencies = agencies
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
