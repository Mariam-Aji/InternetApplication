
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data.Common;
using System.Security.Claims;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;
using WebAPI.Application.Services;
using WebAPI.Domain.Entities;
using WebAPI.Hubs;


namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GovernmentEmployeeController : ControllerBase
    {
        private readonly IGovermentEmployeeService _service;
        private readonly IHubContext<ComplaintHub> _hub;


        public GovernmentEmployeeController(IGovermentEmployeeService service, IHubContext<ComplaintHub> hub)
        {
            _service = service;
            _hub = hub;
        }


        [Authorize(Roles = "GovernmentEmployee")]
        [HttpGet("GovernmentEmployeeComplaint")]
        public async Task<IActionResult> GetEmployeeComplant()
        {
            var GovernmentEmployeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (GovernmentEmployeeId == null)
            {
                return Unauthorized(new
                {
                    status = 401,
                    message = "unauthorize"
                });
            }

            int userId = int.Parse(GovernmentEmployeeId);

            var complaints = await _service.GetEmployeeComplaintsAsync(userId);

            return Ok(new
            {
                status = 200,
                total = complaints.Count,
                complaints = complaints.Select(c => new
                {
                    id = c.Id,
                    Date = c.ComplaintDate,
                    complaintsType = c.ComplaintType,
                    location = c.Location,
                    description = c.Description,
                    userId = c.UserId,
                    user = new
                    {
                        id = c.User.Id,
                        name = c.User.FullName,
                        email = c.User.Email
                    },
                    image1 = c.Image1,
                    image2 = c.Image2,
                    image3 = c.Image3,
                    PdfFile = c.PdfFile,

                    governmentAgencyName = c.GovernmentAgency.AgencyName,
                    statusId = c.ComplaintStatusId,
                    statusName = c.ComplaintStatus.StatusName,
                })
            });
        }

        [Authorize(Roles = "GovernmentEmployee")]
        [HttpPost("RequestEdit")]
        public async Task<IActionResult> RequestEdit([FromForm] int complaintId)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var locked = await _service.LockComplaintAsync(employeeId, complaintId);

            if (!locked)
            {
                return Conflict(new
                {
                    message = "لا يمكن تعديل الشكوى لأنها قيد المعالجة من قبل موظف آخر."
                });
            }

            return Ok(new
            {
                message = "تم حجز الشكوى ويمكنك تعديلها الآن",
                complaintId
            });
        }

        [Authorize(Roles = "GovernmentEmployee")]
        [HttpPut("UpdateComplaintStatus")]
        public async Task<IActionResult> UpdateComplaintStatus([FromForm] EmployeeStatusUpdateDto dto)
        {

            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

           
            var locked = await _service.LockComplaintAsync(employeeId, dto.ComplaintId);

            if (!locked)
            {
                return Conflict(new
                {
                    message = "لا يمكن تعديل الشكوى لأنها قيد المعالجة من قبل موظف آخر."
                });
            }

            var result = await _service.UpdateStatusAsync(dto.ComplaintId, dto.NewStatusId);
            if (!result.Success)
                return NotFound(new { message = "الشكوى غير موجودة" });

            await _service.UnlockComplaintAsync(dto.ComplaintId);

            if (result.CitizenId != null)
            {
                await _hub.Clients.User(result.CitizenId.ToString())
                    .SendAsync("ComplaintStatusChanged", new
                    {
                        complaintId = dto.ComplaintId,
                        newStatusId = dto.NewStatusId,
                        statusName = result.StatusName, 
                        message = $"تم تحديث حالة الشكوى الخاصة بك إلى: {result.StatusName}"
                    });
            }

            return Ok(new
            {
                message = "تم تعديل حالة الشكوى وفك القفل وإرسال إشعار للمواطن",
                complaintId = dto.ComplaintId,
                newStatusId = dto.NewStatusId,
                statusName = result.StatusName 
            });
        }
        [Authorize(Roles = "GovernmentEmployee")]
        [HttpPost("AddComplaintNote")]
        public async Task<IActionResult> AddComplaintNote([FromForm] AddComplaintNoteDto dto)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var locked = await _service.LockComplaintAsync(employeeId, dto.ComplaintId);
            if (!locked)
            {
                return Conflict(new { message = "لا يمكن تعديل الشكوى لأنها قيد المعالجة من قبل موظف آخر." });
            }

            var result = await _service.AddNoteAsync(dto.ComplaintId, dto.Notes);

            await _service.UnlockComplaintAsync(dto.ComplaintId);

            if (!result.Success)
                return NotFound(new { message = "الشكوى غير موجودة" });

            
            if (result.CitizenId != null)
            {
                await _hub.Clients.User(result.CitizenId.ToString())
                    .SendAsync("ComplaintNoteAdded", new
                    {
                        complaintId = dto.ComplaintId,
                        note = dto.Notes,
                        message = "تمت إضافة ملاحظة جديدة إلى الشكوى الخاصة بك."
                    });
            }

            return Ok(new
            {
                message = "تمت إضافة الملاحظة وإرسال إشعار للمواطن",
                complaintId = dto.ComplaintId,
                notes = dto.Notes
            });
        }

    }
}
