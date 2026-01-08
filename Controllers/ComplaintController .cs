using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
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
        public async Task<IActionResult> CreateComplaint(
     [FromRoute] int governmentAgencyId,
     [FromForm] ComplaintRequest request)
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

            var statusName = await _repo.GetComplaintStatusNameAsync(
                complaint.ComplaintStatusId ?? 1);

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

                  
                    complaint.Images,

                    complaint.PdfFile,
                    complaint.ComplaintDate
                }
            });
        }

        [Authorize(Roles = "Citizen")]
        [HttpPut("update-files/{id}")]
        public async Task<IActionResult> UpdateComplaintFiles(
            [FromRoute] int id,
            [FromForm] UpdateComplaintFilesDto request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized(new { message = "المستخدم غير مصرح له" });

            int userId = int.Parse(userIdString);

            var result = await _service.UpdateComplaintFilesAsync(id, userId, request);

            if (!result)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "فشل تحديث الملفات. قد تكون الشكوى غير موجودة أو أنك لا تملك صلاحية تعديلها."
                });
            }

            return Ok(new
            {
                status = 200,
                message = "تم تحديث الصور والملفات بنجاح"
            });
        }
        [Authorize(Roles = "Admin")] 
        [HttpGet("statistics")]
        public async Task<IActionResult> GetComplaintStatistics()
        {
            try
            {
                var stats = await _service.GetComplaintsStatisticsAsync();

                if (stats == null)
                {
                    return NotFound(new
                    {
                        status = 404,
                        message = "لا توجد بيانات متاحة حالياً"
                    });
                }

                return Ok(new
                {
                    status = 200,
                    message = "تم جلب الإحصائيات بنجاح",
                    data = stats
                });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new
                {
                    status = 500,
                    message = "حدث خطأ داخلي أثناء معالجة الإحصائيات",
                    error = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{complaintId}/history")]
        public async Task<IActionResult> GetComplaintHistory(int complaintId)
        {
            var history = await _service.GetHistoryByComplaintIdAsync(complaintId);

            if (history == null || !history.Any())
            {
                return NotFound(new { message = "لا يوجد سجل تاريخي لهذه الشكوى" });
            }

            return Ok(history);
        }
        [Authorize(Roles = "Admin")] 
        [HttpGet("history/all")]
        public async Task<IActionResult> GetAllSystemHistory()
        {
            var history = await _service.GetAllComplaintsHistoryAsync();

            if (history == null || !history.Any())
            {
                return Ok(new { message = "سجل الحركات فارغ حالياً" });
            }

            return Ok(history);
        }
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======
        [Authorize(Roles = "Admin")] 
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
        [HttpGet("system-monitor")]
        public async Task<IActionResult> GetSystemMonitor()
        {
            try
            {
                var metrics = await _service.GetSystemPerformanceAsync();
<<<<<<< HEAD
                return Ok(new
                {
                    status = 200,
                    message = "تقرير أداء النظام الحالي",
                    performance = metrics
                });
=======
                return Ok(metrics);
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
            }
            catch (Exception ex)
            {
                return StatusCode(500, "حدث خطأ أثناء استخراج تقرير الأداء: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("AllComplaint")]
        public async Task<IActionResult> GetAllComplaint()
        {
            
            var complaints = await _service.GetAllComplaintsAsync();

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

<<<<<<< HEAD
                  
=======
                    // 🔴 التعديل هنا فقط
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
                    Images = JsonSerializer.Deserialize<List<string>>(c.Images),

                    PdfFile = c.PdfFile,
                    governmentAgencyName = c.GovernmentAgency.AgencyName,
                    statusId = c.ComplaintStatusId,
                    statusName = c.ComplaintStatus.StatusName,
                })
            });
        }

<<<<<<< HEAD
        [HttpGet("performance-metrics")]
        public async Task<IActionResult> GetPerformance()
        {
            var metrics = await _repo.GetPerformanceMetricsAsync();
            return Ok(metrics);
        }
=======

>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
    }


}
