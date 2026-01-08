<<<<<<< HEAD
﻿using System;

namespace WebAPI.Application.DTOs
{
    public class PerformanceMetricsDto
    {
        public string MemoryUsageMB { get; set; } = string.Empty;
        public string CpuUsagePercentage { get; set; } = string.Empty;
        public int ActiveThreads { get; set; }
        public DateTime ReportGeneratedAt { get; set; }
    }
=======
﻿public class PerformanceMetricsDto
{
    public int TotalComplaints { get; set; }
    public double CompletionRate { get; set; } 
    public int PendingComplaints { get; set; }
    public double AverageProcessingTimeDays { get; set; } 
    public long SystemMemoryUsageBytes { get; set; } 
    public DateTime ReportGeneratedAt { get; set; }
>>>>>>> de431c5bc2c01e485946d971fe16b71df9778b76
}