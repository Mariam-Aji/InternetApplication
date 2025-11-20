<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Mvc;
>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
using WebAPI.Application.DTOs;
using WebAPI.Application.Services;

namespace WebAPI.Application.Interfaces;

public interface IAuthService
{
<<<<<<< HEAD
    Task RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task SendOtpAsync(string email);
    Task VerifyOtpAsync(VerifyOtpRequest request);
    Task CreateGovernmentEmployeeAsync(string adminEmail,string FullName, string employeeEmail, int department_id, string password);
=======
    Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, string Message, AuthResponse? Data)> LoginAsync(LoginRequest request);
    Task SendOtpAsync(string email);
    Task<(bool Success, string Message)> VerifyOtpAsync(VerifyOtpRequest request);
    Task<(bool Success, string Message)> CreateGovernmentEmployeeAsync(string adminEmail,string FullName, string employeeEmail, int department_id, string password);
>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
}
