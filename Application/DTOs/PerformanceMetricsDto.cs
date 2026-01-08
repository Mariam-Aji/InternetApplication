using System;

namespace WebAPI.Application.DTOs
{
    public class PerformanceMetricsDto
    {
        public string MemoryUsageMB { get; set; } = string.Empty;
        public string CpuUsagePercentage { get; set; } = string.Empty;
        public int ActiveThreads { get; set; }
        public DateTime ReportGeneratedAt { get; set; }
    }
}