using HRMS.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HRMS.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(
                emailSettings["SenderName"], 
                emailSettings["SenderEmail"]
            ));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(
                emailSettings["SmtpServer"],
                int.Parse(emailSettings["SmtpPort"]!),
                SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(
                emailSettings["Username"],
                emailSettings["Password"]
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {To}", to);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            return false;
        }
    }

    public async Task<bool> SendOtpEmailAsync(string email, string otp)
    {
        var subject = "HRMS - Mã OTP Reset Password";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                    <h2 style='color: #0d6efd;'>Yêu cầu Reset Password</h2>
                    <p>Bạn đã yêu cầu reset password cho tài khoản HRMS của mình.</p>
                    <p>Mã OTP của bạn là:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; letter-spacing: 5px; color: #0d6efd; border-radius: 5px;'>
                        {otp}
                    </div>
                    <p style='margin-top: 20px;'>Mã OTP này có hiệu lực trong <strong>5 phút</strong>.</p>
                    <p style='color: #dc3545;'><strong>Lưu ý:</strong> Nếu bạn không yêu cầu reset password, vui lòng bỏ qua email này.</p>
                    <hr style='margin: 20px 0;'>
                    <p style='color: #6c757d; font-size: 12px;'>Email này được gửi tự động từ hệ thống HRMS. Vui lòng không trả lời email này.</p>
                </div>
            </body>
            </html>
        ";

        return await SendEmailAsync(email, subject, body);
    }

    public async Task<bool> SendNotificationEmailAsync(SendEmailDto dto)
    {
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                    <h2 style='color: #0d6efd;'>HRMS Notification</h2>
                    <div style='margin-top: 20px; line-height: 1.6;'>
                        {dto.Body}
                    </div>
                    <hr style='margin: 20px 0;'>
                    <p style='color: #6c757d; font-size: 12px;'>Email này được gửi từ hệ thống HRMS.</p>
                </div>
            </body>
            </html>
        ";

        return await SendEmailAsync(dto.RecipientEmail, dto.Subject, body);
    }
}

