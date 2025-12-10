namespace WebAPI.Domain.Entities
{
    public class Complaint
    {
        public int Id { get; set; }
        public string ComplaintType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public string Image1 { get; set; }
        public string? Image2 { get; set; } = null;
        public string? Image3 { get; set; } = null;

        public string PdfFile { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; } = null;

        public int? GovernmentAgencyId { get; set; }
        public GovernmentAgency? GovernmentAgency { get; set; } = null;

        public int? ComplaintStatusId { get; set; } = 1;

        public ComplaintStatus? ComplaintStatus { get; set; } = null;
        public ComplaintAdministration? ComplaintAdministration { get; set; }
        public DateOnly? ComplaintDate { get; set; }
        public ICollection<ComplaintHistory>? Histories { get; set; } = null;


    }

}