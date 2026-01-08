<<<<<<< HEAD
﻿//using WebAPI.Migrations;
=======
﻿using WebAPI.Migrations;
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76

namespace WebAPI.Infrastructure.Repositories
{
    public interface IComplaintHistoryRepository
    {
        Task AddHistoryAsync(int complaintId, int employeeId, string actionType, string? newValue);
        Task<IEnumerable<WebAPI.Domain.Entities.ComplaintHistory>> GetComplaintHistoriesAsync(int complaintId);

        Task<IEnumerable<WebAPI.Domain.Entities.ComplaintHistory>> GetAllHistoriesAsync();

    }
}
