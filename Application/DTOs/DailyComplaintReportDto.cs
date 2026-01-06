namespace WebAPI.Application.DTOs
{
    public class DailyComplaintReportDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public List<string> ImagePaths { get; set; }

        public DateOnly? Date { get; set; }
        public List<ComplaintHistoryReportDto> History { get; set; }
    }
}
