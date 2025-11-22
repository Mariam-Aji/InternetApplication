using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Services
{
    public class ComplaintStatusService : IComplaintStatusService
    {
        private readonly IComplaintStatusRepository _repo;

        public ComplaintStatusService(IComplaintStatusRepository repo)
        {
            _repo = repo;
        }

        
        public async Task<List<ComplaintStatus>> GetAllStatusesAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<List<Complaint>> GetUserComplaintsAsync(int userId)
        {
            return await _repo.GetComplaintsForUserAsync(userId);
        }

    }
}
