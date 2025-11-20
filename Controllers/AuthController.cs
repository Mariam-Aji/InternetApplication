using WebAPI.Application.Interfaces;
using WebAPI.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) { _auth = auth; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterRequest req)
    {
        await _auth.RegisterAsync(req);
        return Ok(new { Message = "Registered. OTP sent to email." });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpRequest req)
    {
        await _auth.VerifyOtpAsync(req);
        return Ok(new { Message = "Email verified" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequest req)
    {
        var result = await _auth.LoginAsync(req);
        if (result == null)
            return Unauthorized();

        return Ok(result);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("admin/create-employee")]
    public async Task<IActionResult> CreateEmployee([FromForm] Application.DTOs.CreateEmployeeDto dto)
    {
        var adminEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? throw new Exception("Admin email not found in token");
        await _auth.CreateGovernmentEmployeeAsync(adminEmail, dto.FullName,dto.Email, dto.Department_id, dto.Password);
        return Ok(new { Message = "Employee created" });
    }
}
