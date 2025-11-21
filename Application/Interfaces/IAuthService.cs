using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs;
using WebAPI.Application.Services;

namespace WebAPI.Application.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, string Message, AuthResponse? Data)> LoginAsync(LoginRequest request);
    Task SendOtpAsync(string email);
    Task<(bool Success, string Message)> VerifyOtpAsync(VerifyOtpRequest request);
    Task<(bool Success, string Message)> CreateGovernmentEmployeeAsync(string adminEmail, string FullName, string employeeEmail, int department_id, string password);
}
