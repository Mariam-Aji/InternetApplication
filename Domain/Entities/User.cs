

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
<<<<<<< HEAD
    public ICollection<Complaint>? Complaints { get; set; } = null;
=======
>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
}
