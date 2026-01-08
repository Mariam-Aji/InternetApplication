using WebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Infrastructure.Seeders
{
    public static class ComplaintStatusSeeder
    {
        public static void SeedComplaintStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComplaintStatus>().HasData(
                new ComplaintStatus { Id = 1, StatusName = "جديدة" },
                new ComplaintStatus { Id = 2, StatusName = "قيد المعالجة" },
                new ComplaintStatus { Id = 3, StatusName = "منجزة" },
                new ComplaintStatus { Id = 4, StatusName = "مرفوضة" }
            );
        }
    }
}
