namespace WebAPI.Domain.Entities;

public class OtpCode
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Code { get; set; } = null!;
    public DateTime ExpireAt { get; set; }
    public bool Used { get; set; } = false;
}
