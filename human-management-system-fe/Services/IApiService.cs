using human_management_system_fe.Models;

namespace human_management_system_fe.Services;

public interface IApiService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> LogoutAsync();
    Task<UserProfileDto?> GetUserProfileAsync(int userId);
    Task<EmailResponseDto?> SendEmailAsync(SendEmailDto emailDto);
}

