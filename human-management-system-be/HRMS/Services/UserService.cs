using HRMS.DTOs;
using HRMS.Repositories;

namespace HRMS.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository, 
        IOtpService otpService, 
        IEmailService emailService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _otpService = otpService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetUserWithEmployeeDetailsAsync(userId);
            
            if (user == null)
                return null;

            var profile = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            // Thêm thông tin employee nếu có
            if (user.EmployeeIdNavigation != null)
            {
                var employee = user.EmployeeIdNavigation;
                profile.DepartmentId = employee.DepartmentId;
                profile.DepartmentName = employee.Department?.Name;
                profile.HireDate = employee.HireDate.HasValue 
                    ? employee.HireDate.Value.ToDateTime(TimeOnly.MinValue) 
                    : null;
                profile.Salary = employee.Salary;
                profile.Address = employee.Address;
            }

            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile for userId {UserId}", userId);
            return null;
        }
    }

    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", email);
                // Trả về true để không tiết lộ thông tin user có tồn tại hay không
                return true;
            }

            if (user.IsActive == false)
            {
                _logger.LogWarning("Password reset requested for inactive user: {Email}", email);
                return false;
            }

            // Generate and store OTP
            var otp = _otpService.GenerateOtp();
            _otpService.StoreOtp(email, otp);

            // Send OTP via email
            var emailSent = await _emailService.SendOtpEmailAsync(email, otp);
            
            if (!emailSent)
            {
                _logger.LogError("Failed to send OTP email to {Email}", email);
                _otpService.RemoveOtp(email);
                return false;
            }

            _logger.LogInformation("OTP sent successfully to {Email}", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting password reset for {Email}", email);
            return false;
        }
    }

    public async Task<bool> ResetPasswordWithOtpAsync(VerifyOtpDto dto)
    {
        try
        {
            // Validate OTP
            if (!_otpService.ValidateOtp(dto.Email, dto.Otp))
            {
                _logger.LogWarning("Invalid or expired OTP for {Email}", dto.Email);
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            
            if (user == null)
            {
                _logger.LogWarning("User not found for email {Email}", dto.Email);
                return false;
            }

            // Hash new password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            // Update password
            var updated = await _userRepository.UpdatePasswordAsync(user.Id, passwordHash);
            
            if (updated)
            {
                // Remove OTP after successful reset
                _otpService.RemoveOtp(dto.Email);
                _logger.LogInformation("Password reset successfully for {Email}", dto.Email);
            }

            return updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for {Email}", dto.Email);
            return false;
        }
    }
}

