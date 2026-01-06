using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintReportService
    {
        Task<List<DailyComplaintReportDto>> GetTodayComplaintsAsync();
        Task<List<DailyComplaintReportDto>> GetLast7DaysComplaintsAsync();
    }

}
