using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Db;

namespace WebAPI.Infrastructure.Repositories
{
    public class GovermentEmployeeRepository : IGovermentEmployeeRepositry
    {
        private readonly AppDbContext _db;
       
        public GovermentEmployeeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Complaint>> GetComplaintsForEmployeeAsync(int userId)
        {
            var employee = await _db.Users
      .FirstOrDefaultAsync(u => u.Id == userId);

            if (employee == null || employee.Department_id == null)
                return new List<Complaint>();

            var departmentId = employee.Department_id;

         
            return await _db.Complaints
                .Include(c=>c.User)
                .Include(c => c.ComplaintStatus)
                .Include(c => c.GovernmentAgency)
                .Where(c => c.GovernmentAgencyId == departmentId)
                .ToListAsync();
        }
        public async Task<bool> LockComplaintAsync(int complaintId, int employeeId)
        {
            var existing = await _db.ComplaintLocks
                .FirstOrDefaultAsync(l => l.ComplaintId == complaintId);

            //قفل غير منتهي 
            if (existing != null && existing.ExpiresAt > DateTime.Now)
            {
                if (existing.EmployeeId == employeeId)
                    return true; // محجوزة من نفس الموظف

                return false; // محجوزة من موظف آخر
            }

            // حذف قفل منتهي
            if (existing != null)
                _db.ComplaintLocks.Remove(existing);

            //  قفل جديد
            var newLock = new ComplaintLock
            {
                ComplaintId = complaintId,
                EmployeeId = employeeId,
                LockedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(10)
            };

            await _db.ComplaintLocks.AddAsync(newLock);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task UnlockComplaintAsync(int complaintId)
        {
            var lockItem = await _db.ComplaintLocks
                .FirstOrDefaultAsync(l => l.ComplaintId == complaintId);

            if (lockItem != null)
            {
                _db.ComplaintLocks.Remove(lockItem);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<(bool Success, int? CitizenId, string StatusName)> UpdateStatusAsync(int complaintId, int newStatusId)
        {

            var complaint = await _db.Complaints.FirstOrDefaultAsync(c => c.Id == complaintId);
          
            if (complaint == null) return (false, null, null);
            var statusExists = await _db.ComplaintStatuses.AnyAsync(s => s.Id == newStatusId);
            if (!statusExists) return (false, complaint.UserId, null);
            complaint.ComplaintStatusId = newStatusId;
            
            await _db.SaveChangesAsync();

           
            var statusName = await _db.ComplaintStatuses
                .Where(s => s.Id == newStatusId) .Select(s => s.StatusName) .FirstOrDefaultAsync();

            return (true, complaint.UserId, statusName);
        }


        public async Task<(bool Success, int? CitizenId)> AddNoteAsync(int complaintId, string note)
        {
            var complaint = await _db.Complaints
                .FirstOrDefaultAsync(c => c.Id == complaintId);

            if (complaint == null)
                return (false, null);

            var adminRecord = new ComplaintAdministration
            {
                ComplaintId = complaintId,
                Notes = note,
                GovernmentAgencyId = complaint.GovernmentAgencyId
            };

            _db.ComplaintAdministrations.Add(adminRecord);
            await _db.SaveChangesAsync();

            return (true, complaint.UserId);
        }



    }
}
