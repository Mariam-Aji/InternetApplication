namespace WebAPI.Application.DTOs
{
    public class ComplaintRequest
    {
        public string ComplaintType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public List<IFormFile> Images { get; set; } = new();

        public IFormFile PdfFile { get; set; }

        public int UserId { get; set; }
        public int GovernmentAgencyId { get; set; }
        public int ComplaintStatusId { get; set; } = 1;
        public DateOnly? ComplaintDate { get; set; }



    }
}

