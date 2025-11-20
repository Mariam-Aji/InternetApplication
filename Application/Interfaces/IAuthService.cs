using WebAPI.Application.DTOs;
using WebAPI.Application.Services;

namespace WebAPI.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task SendOtpAsync(string email);
    Task VerifyOtpAsync(VerifyOtpRequest request);
    Task CreateGovernmentEmployeeAsync(string adminEmail,string FullName, string employeeEmail, int department_id, string password);
}
