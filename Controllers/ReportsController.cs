using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WebAPI.Application.Interfaces;
using WebAPI.Application.Services;

<<<<<<< HEAD

=======
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/reports")]
<<<<<<< HEAD
    
=======
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
    public class ReportsController : ControllerBase
    {
        private readonly IComplaintReportService _reportService;

        public ReportsController(IComplaintReportService reportService)
        {
            _reportService = reportService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("daily-complaints")]
        public async Task<IActionResult> GetDailyComplaintsReport()
        {
            var complaints = await _reportService.GetTodayComplaintsAsync();

            var document = new DailyComplaintsReport(complaints);
            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", "Daily_Complaints_Report.pdf");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("weekly-complaints")]
        public async Task<IActionResult> GetWeeklyComplaintsReport()
        {
            var complaints = await _reportService.GetLast7DaysComplaintsAsync();

            var document = new WeekComplantReport(complaints);
            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", "Weekly_Complaints_Report.pdf");
        }

    }

}
