using HRMS.DTOs;

namespace HRMS.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> ValidateUserAsync(string username, string password);
    Task<LoginResponseDto?> RegisterAsync(CreateRegisterDto dto);
}

