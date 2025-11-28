namespace WebAPI.Application.DTOs;

public record RegisterRequest(string FullName,string Email, string Password,string ConfirmPassword);
public record LoginRequest(string Email, string Password);
public record VerifyOtpRequest(string Email, string Code);
public record resendOtpRequest(string Email);
public record CreateEmployeeDto(string FullName,string Email, int Department_id, string Password);
