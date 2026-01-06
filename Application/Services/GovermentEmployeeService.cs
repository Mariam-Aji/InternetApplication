using Microsoft.AspNetCore.SignalR;
using WebAPI.Application.Interfaces;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Repositories;
using WebAPI.Hubs;

namespace WebAPI.Application.Services
{
    public class GovermentEmployeeService : IGovermentEmployeeService
    {
        private readonly IGovermentEmployeeRepositry _repo;
        private readonly IComplaintHistoryRepository _history;
        private readonly IHubContext<ComplaintHub> _hub;  

        public GovermentEmployeeService(
            IGovermentEmployeeRepositry repo,
            IComplaintHistoryRepository history,
            IHubContext<ComplaintHub> hub)  
        {
            _repo = repo;
            _history = history;
            _hub = hub;
        }

        public async Task<List<Complaint>> GetEmployeeComplaintsAsync(int userId)
        {
            return await _repo.GetComplaintsForEmployeeAsync(userId);
        }

        public async Task<bool> LockComplaintAsync(int employeeId, int complaintId)
        { return await _repo.LockComplaintAsync(complaintId, employeeId); }

        public async Task UnlockComplaintAsync(int complaintId)
        { await _repo.UnlockComplaintAsync(complaintId); }

        public async Task<(bool Success, int? CitizenId, string StatusName)> UpdateStatusAsync(int complaintId, int newStatusId, int employeeId)
        {
            var result = await _repo.UpdateStatusAsync(complaintId, newStatusId);

            if (result.Success)
            {
                await _history.AddHistoryAsync(
                    complaintId,
                    employeeId,
                    "StatusChanged",
                    result.StatusName
                );
            }

            return result;
        }

        public async Task<(bool Success, int? CitizenId)> AddNoteAsync(int complaintId, string note, int employeeId)
        {
            var result = await _repo.AddNoteAsync(complaintId, note);

            if (result.Success)
            {
                await _history.AddHistoryAsync(
                    complaintId,
                    employeeId,
                    "NoteAdded",
                    note
                );
            }

            return result;
        }

        public async Task<bool> RequestAdditionalInfoAsync(
            int complaintId,
            int citizenId,
            string message,
            int employeeId)
        {
            var exists = await _repo.ComplaintExistsAsync(complaintId);
            if (!exists)
                return false;

            await _history.AddHistoryAsync(
                complaintId,
                employeeId,
                "AdditionalInfoRequested",
                message
            );

            await _hub.Clients.User(citizenId.ToString())
                .SendAsync("AdditionalInfoRequested", new
                {
                    complaintId,
                    message,
                    requestedBy = employeeId
                });

            return true;
        }
    }
}
