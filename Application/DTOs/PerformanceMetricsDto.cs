<<<<<<< HEAD

﻿using System;

namespace WebAPI.Application.DTOs
{


    public class PerformanceMetricsDto
    {
        public int TotalComplaints { get; set; }
        public double CompletionRate { get; set; }
        public int PendingComplaints { get; set; }
        public double AverageProcessingTimeDays { get; set; }
        public long SystemMemoryUsageBytes { get; set; }
        public DateTime ReportGeneratedAt { get; set; }

    } }
=======
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
>>>>>>> f8b3d41 (Performance: optimize server to handle up to 100 concurrent users)
