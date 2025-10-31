using HRMS.DTOs;

namespace HRMS.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
    Task<bool> SendOtpEmailAsync(string email, string otp);
    Task<bool> SendNotificationEmailAsync(SendEmailDto dto);
}

