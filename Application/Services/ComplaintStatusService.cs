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

        public async Task AddStatusAsync(ComplaintStatus status)
        {
            var exists = await _repo.GetByNameAsync(status.StatusName);
            if (exists != null)
                throw new Exception("هذه الحالة موجودة مسبقًا");

            await _repo.AddAsync(status);
        }

        public async Task<List<ComplaintStatus>> GetAllStatusesAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
