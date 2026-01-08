using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Db.Configurations
{
    public class GovernmentAgencyConfiguration : IEntityTypeConfiguration<GovernmentAgency>
    {
        public void Configure(EntityTypeBuilder<GovernmentAgency> builder)
        {
            builder.Property(a => a.AgencyName).IsRequired().HasMaxLength(200);

            builder.HasData(
                new GovernmentAgency { Id = 1, AgencyName = "وزارة الصحة" },
                new GovernmentAgency { Id = 2, AgencyName = "وزارة التعليم" },
                new GovernmentAgency { Id = 3, AgencyName = "وزارة الداخلية" },
                new GovernmentAgency { Id = 4, AgencyName = "وزارة العمل والشؤون الاجتماعية" },
                new GovernmentAgency { Id = 5, AgencyName = "أمانة العاصمة" },
                new GovernmentAgency { Id = 6, AgencyName = "هيئة النزاهة" }
            );
        }
    }
}