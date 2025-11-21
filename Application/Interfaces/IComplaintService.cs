using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities
;

namespace WebAPI.Application.Interfaces
{
    public interface IComplaintService
    {
        Task<Complaint> AddComplaintAsync(ComplaintRequest request);

    }
}
