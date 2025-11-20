namespace WebAPI.Domain.Entities
{
    public class ComplaintStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        public ICollection<Complaint>? Complaints { get; set; } = null;
    }

}
