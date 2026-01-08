

namespace WebAPI.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "Citizen"; // Admin | GovernmentEmployee | Citizen
    public bool IsEmailConfirmed { get; set; } = false;
    public int? Department_id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Lockout fields
    public int FailedLoginAttempts { get; set; } = 0;
    public bool IsLockedOut { get; set; } = false;
    public DateTime? LockoutEnd { get; set; }

    public ICollection<Complaint>? Complaints { get; set; } = null;

}
