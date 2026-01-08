namespace WebAPI.Domain.Entities
{
    public class ComplaintLock
       {
            public int Id { get; set; }
            public int ComplaintId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime LockedAt { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    
}
