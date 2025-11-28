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
        var result = await _auth.RegisterAsync(req);

        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromForm] resendOtpRequest req)
    {
        var result = await _auth.ResendOtpAsync(req.Email);

        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpRequest req)
    {
        var result = await _auth.VerifyOtpAsync(req);

        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequest req)
    {
        var result = await _auth.LoginAsync(req);

        if (!result.Success)
        {
            if (result.Message.Contains("locked", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { Message = result.Message });

            return Unauthorized(new { Message = result.Message });
        }

        return Ok(result.Data);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/create-employee")]
    public async Task<IActionResult> CreateEmployee([FromForm] CreateEmployeeDto dto)
    {
        var adminEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(adminEmail))  return Unauthorized(new { Message = "Admin email not found in token" });
        var result = await _auth.CreateGovernmentEmployeeAsync(adminEmail, dto.FullName, dto.Email, dto.Department_id, dto.Password);
        if (!result.Success)
        {

            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }
}





