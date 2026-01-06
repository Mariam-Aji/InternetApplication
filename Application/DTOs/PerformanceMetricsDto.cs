public class PerformanceMetricsDto
{
    public int TotalComplaints { get; set; }
    public double CompletionRate { get; set; } 
    public int PendingComplaints { get; set; }
    public double AverageProcessingTimeDays { get; set; } 
    public long SystemMemoryUsageBytes { get; set; } 
    public DateTime ReportGeneratedAt { get; set; }
}