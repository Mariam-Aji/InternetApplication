public class ComplaintHistoryDto
{
    public int Id { get; set; }
    public int ComplaintId { get; set; }
    public string? ActionType { get; set; }
    public string? NewValue { get; set; }
    public DateTime ActionDate { get; set; }
    public string? PerformedBy { get; set; } 
}