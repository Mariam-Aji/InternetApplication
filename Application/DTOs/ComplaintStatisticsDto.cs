public class ComplaintStatisticsDto
{
    public int TotalComplaints { get; set; }
    public int NewComplaints { get; set; }        // Id = 1 (جديدة)
    public int UnderProcess { get; set; }        // Id = 2 (قيد المعالجة)
    public int Completed { get; set; }           // Id = 3 (منجزة)
    public int Rejected { get; set; }            // Id = 4 (مرفوضة)
}