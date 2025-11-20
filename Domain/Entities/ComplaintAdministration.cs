using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Domain.Entities
{
    public class ComplaintAdministration
    {
        public int Id { get; set; } 
        public int ComplaintId { get; set; }
        public Complaint? Complaint { get; set; }

        public int? GovernmentAgencyId { get; set; }
        public GovernmentAgency? GovernmentAgency { get; set; }
    }
}
