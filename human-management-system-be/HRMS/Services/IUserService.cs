using HRMS.DTOs;

namespace HRMS.Services;

public interface IUserService
{
    Task<UserProfileDto?> GetUserProfileAsync(int userId);
    Task<bool> RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordWithOtpAsync(VerifyOtpDto dto);
}

