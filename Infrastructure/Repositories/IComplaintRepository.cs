using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintRepository
    {
        Task AddAsync(Complaint complaint);
        Task<bool> GovernmentAgencyExistsAsync(int id);
        Task<string?> GetComplaintStatusNameAsync(int statusId);
        Task<int?> GetCitizenIdByComplaintIdAsync(int complaintId);
        Task<Complaint> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Complaint complaint);

        Task AddHistoryAsync(ComplaintHistory history);
        Task<Dictionary<int, int>> GetComplaintsCountByStatusAsync();
        Task<IEnumerable<ComplaintHistory>> GetComplaintHistoriesAsync(int complaintId);
        Task<PerformanceMetricsDto> GetPerformanceMetricsAsync();
        Task<List<Complaint>> GetALLComplaintsAsync();
    }
}
