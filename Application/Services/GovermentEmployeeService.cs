using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI.Application.Services
{
    public class GovermentEmployeeService : IGovermentEmployeeService
    {
        private readonly IGovermentEmployeeRepositry _repo;

        public GovermentEmployeeService(IGovermentEmployeeRepositry repo)
        {
            _repo = repo;
        }

        public async Task<List<Complaint>> GetEmployeeComplaintsAsync(int userId)
        {
            return await _repo.GetComplaintsForEmployeeAsync(userId);
        }

        public async Task<bool> LockComplaintAsync(int employeeId, int complaintId)
        { return await _repo.LockComplaintAsync(complaintId, employeeId); }

        public async Task UnlockComplaintAsync(int complaintId)
        {  await _repo.UnlockComplaintAsync(complaintId); }

        public async Task<(bool Success, int? CitizenId, string StatusName)> UpdateStatusAsync(int complaintId, int newStatusId)
        {
            return await _repo.UpdateStatusAsync(complaintId, newStatusId);
        }
        public async Task<(bool Success, int? CitizenId)> AddNoteAsync(int complaintId, string note)
        {
            return await _repo.AddNoteAsync(complaintId, note);
        }
    }

    }

