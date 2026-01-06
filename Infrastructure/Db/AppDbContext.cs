using WebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure.Seeders;

namespace WebAPI.Infrastructure.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<OtpCode> OtpCodes { get; set; } = null!;

    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<GovernmentAgency> GovernmentAgencies { get; set; }
    public DbSet<ComplaintStatus> ComplaintStatuses { get; set; }
    public DbSet<ComplaintAdministration> ComplaintAdministrations { get; set; }
    public DbSet<ComplaintLock> ComplaintLocks { get; set; }
    public DbSet<ComplaintHistory> ComplaintHistories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<ComplaintStatus>()
    .Property(c => c.StatusName)
    .IsUnicode(true);
        ComplaintStatusSeeder.SeedComplaintStatuses(modelBuilder);


        modelBuilder.Entity<GovernmentAgency>()
            .HasMany(a => a.Complaints)
            .WithOne(c => c.GovernmentAgency)
            .HasForeignKey(c => c.GovernmentAgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComplaintStatus>()
            .HasMany(s => s.Complaints)
            .WithOne(c => c.ComplaintStatus)
            .HasForeignKey(c => c.ComplaintStatusId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();

            entity.HasMany(u => u.Complaints)
                  .WithOne(c => c.User)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }

       );
        modelBuilder.Entity<Complaint>()
     .HasOne(c => c.ComplaintAdministration)
     .WithOne(ca => ca.Complaint)
     .HasForeignKey<ComplaintAdministration>(ca => ca.ComplaintId)
     .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ComplaintAdministration>()
            .HasOne(ca => ca.GovernmentAgency)
            .WithMany()
            .HasForeignKey(ca => ca.GovernmentAgencyId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ComplaintHistory>(entity =>
        {
           
            entity.HasOne(h => h.Complaint)
          .WithMany(c => c.Histories)
           .HasForeignKey(h => h.ComplaintId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.Employee)
        .WithMany()
        .HasForeignKey(h => h.EmployeeId)
           .OnDelete(DeleteBehavior.Restrict);
        });

    }
}





