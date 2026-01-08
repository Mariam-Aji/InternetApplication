<<<<<<< HEAD
﻿using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
=======
﻿using WebAPI.Domain.Entities;
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76

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
<<<<<<< HEAD

=======
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
    }
}
