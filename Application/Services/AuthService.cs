using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;
using WebAPI.Application.Validation;
using WebAPI.Domain.Entities;
using WebAPI.Hubs;
using WebAPI.Infrastructure.Db;
using WebAPI.Infrastructure.Email;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;
    private readonly IHubContext<LockoutHub> _hub;

    private const int MaxFailedAttempts = 5;
    private const int LockoutMinutes = 15;

    public AuthService(IUserRepository users, IEmailSender emailSender, IPasswordHasher<User> passwordHasher,
        IConfiguration config, AppDbContext db, IHubContext<LockoutHub> hub)
    {
        _users = users;
        _emailSender = emailSender;
        _passwordHasher = passwordHasher;
        _config = config;
        _db = db;
        _hub = hub;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
    {
        var exists = await _users.GetByEmailAsync(request.Email);
        if (exists != null)
        {
            return (false, "Email already registered.");
        }

        var user = new User
        {
            Email = request.Email,
            Role = "Citizen",
            FullName = request.FullName
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _users.AddAsync(user);
        await SendOtpAsync(user.Email);

        return (true, "Registration successful. OTP sent to email.");
    }

    public async Task<(bool Success, string Message, AuthResponse? Data)> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email);
        if (user == null) return (false, "Invalid credentials", null);

        if (user.IsLockedOut && user.LockoutEnd > DateTime.UtcNow) return (false, $"Account locked until: {user.LockoutEnd}", null);

        if (user.IsLockedOut && user.LockoutEnd <= DateTime.UtcNow)
        {
            user.IsLockedOut = false;
            user.FailedLoginAttempts = 0;
            await _users.UpdateAsync(user);
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= MaxFailedAttempts)
            {
                user.IsLockedOut = true;
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(LockoutMinutes);
                await _users.UpdateAsync(user);

                try
                {
                    await _hub.Clients.User(user.Id.ToString()).SendAsync("AccountLockedOut", new
                    {
                        message = "تم قفل الحساب لمدة 15 دقيقة",
                        lockoutEnd = user.LockoutEnd
                    });
                }
                catch { }

                return (false, "Account locked due to multiple failed attempts", null);
            }

            await _users.UpdateAsync(user); return (false, "Invalid credentials", null);
        }

        user.IsLockedOut = false;
        user.FailedLoginAttempts = 0;
        await _users.UpdateAsync(user);

        if (!user.IsEmailConfirmed)
            return (false, "Email not verified", null);

        var token = GenerateJwt(user);
        var response = new AuthResponse
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                FullName = user.FullName
            }
        };

        return (true, "Login successful", response);
    }
    public async Task SendOtpAsync(string email)
    {
        var user = await _users.GetByEmailAsync(email) ?? throw new Exception("User not found");

        var code = new Random().Next(100000, 999999).ToString();
        var otp = new OtpCode { UserId = user.Id, Code = code, ExpireAt = DateTime.UtcNow.AddMinutes(10) };

        _db.OtpCodes.Add(otp);
        await _db.SaveChangesAsync();

        await _emailSender.SendEmailAsync(email, "OTP Verification",
            $"رمز التحقق هو <b>{code}</b>، صالح لمدة 10 دقائق.");
    }

    public async Task<(bool Success, string Message)> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return (false, "User not found.");
        }

        var otp = await _db.OtpCodes
            .OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync(o => o.UserId == user.Id && !o.Used && o.Code == request.Code);

        if (otp == null || otp.ExpireAt < DateTime.UtcNow)
        {
            return (false, "OTP invalid or expired.");
        }

        otp.Used = true;
        user.IsEmailConfirmed = true;

        await _db.SaveChangesAsync();
        await _users.UpdateAsync(user);

        return (true, "Email verified successfully.");
    }
    public async Task<(bool Success, string Message)> CreateGovernmentEmployeeAsync(string adminEmail, string fullName, string employeeEmail, int department_id, string password)
    {
        var admin = await _users.GetByEmailAsync(adminEmail);
        if (admin == null) return (false, "Admin not found");
        if (admin.Role != "Admin") return (false, "Not allowed");
        var exists = await _users.GetByEmailAsync(employeeEmail);
        if (exists != null) return (false, "Email already exists");
        var user = new User
        {
            FullName = fullName,
            Email = employeeEmail,
            Role = "GovernmentEmployee",
            Department_id = department_id,
            IsEmailConfirmed = true,
            PasswordHash = _passwordHasher.HashPassword(null, password)
        };

        await _users.AddAsync(user);
        return (true, "Employee created successfully");
    }
    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
