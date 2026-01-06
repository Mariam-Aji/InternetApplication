using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities
;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintService
    {
        Task<Complaint> AddComplaintAsync(ComplaintRequest request);
        Task<bool> UpdateComplaintFilesAsync(int complaintId, int userId, UpdateComplaintFilesDto dto);
        Task<ComplaintStatisticsDto> GetComplaintsStatisticsAsync();
        Task<IEnumerable<ComplaintHistoryDto>> GetHistoryByComplaintIdAsync(int complaintId);
        Task<IEnumerable<ComplaintHistoryDto>> GetAllComplaintsHistoryAsync();
        Task<PerformanceMetricsDto> GetSystemPerformanceAsync();
    }
//
}
