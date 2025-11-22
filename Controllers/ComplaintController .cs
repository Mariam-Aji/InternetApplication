using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _service;
        private readonly IAuthService _auth;
        private readonly IComplaintRepository _repo;

        public ComplaintController(IComplaintService service, IAuthService auth, IComplaintRepository repo)
        {
            _service = service;
            _auth = auth;
            _repo = repo;
        }

        [Authorize(Roles = "Citizen")]
        [HttpPost("create/{governmentAgencyId}")]
        public async Task<IActionResult> CreateComplaint([FromRoute] int governmentAgencyId, [FromForm] ComplaintRequest request)
        {
            var citizenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new Exception("Citizen ID not found in token");

            request.UserId = int.Parse(citizenId);
            request.GovernmentAgencyId = governmentAgencyId;

            var complaint = await _service.AddComplaintAsync(request);

            if (complaint == null)
            {
                return NotFound(new
                {
                    status = 404,
                    message = "الجهة الحكومية غير موجودة"
                });
            }

            var statusName = await _repo.GetComplaintStatusNameAsync(complaint.ComplaintStatusId ?? 1);

            return Ok(new
            {
                status = 200,
                message = "تم إنشاء الشكوى بنجاح",
                complaintNumber = complaint.Id,
                complaintStatus = statusName,
                complaint = new
                {
                    complaint.Id,
                    complaint.ComplaintType,
                    complaint.Description,
                    complaint.Location,
                    complaint.UserId,
                    complaint.GovernmentAgencyId,
                    complaint.Image1,
                    complaint.Image2,
                    complaint.Image3,
                    complaint.PdfFile
                }
            });
        }



    }
}
