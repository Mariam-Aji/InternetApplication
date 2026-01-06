namespace WebAPI.Domain.Entities
{
    public class ComplaintHistory
    {
        public int Id { get; set; }

        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }

        public int? EmployeeId { get; set; } 
        public User Employee { get; set; }

        public string ActionType { get; set; } // StatusChanged, NoteAdded, AttachmentAdded
        public string? NewValue { get; set; }   
        public DateTime ActionDate { get; set; }
    }
}
