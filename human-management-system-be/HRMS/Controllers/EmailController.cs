using HRMS.DTOs;
using HRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailService emailService, ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Gửi email notification
    /// </summary>
    [HttpPost("send")]
    public async Task<ActionResult<EmailResponseDto>> SendEmail([FromBody] SendEmailDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _emailService.SendNotificationEmailAsync(dto);

            if (!result)
            {
                return BadRequest(new EmailResponseDto
                {
                    Success = false,
                    Message = "Không thể gửi email. Vui lòng kiểm tra cấu hình email."
                });
            }

            return Ok(new EmailResponseDto
            {
                Success = true,
                Message = "Email đã được gửi thành công",
                SentAt = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return StatusCode(500, new EmailResponseDto
            {
                Success = false,
                Message = "Đã xảy ra lỗi khi gửi email"
            });
        }
    }

    /// <summary>
    /// Kiểm tra trạng thái email service
    /// </summary>
    [HttpGet("status")]
    public ActionResult<object> CheckEmailStatus()
    {
        try
        {
            var configuration = HttpContext.RequestServices.GetService<IConfiguration>();
            var emailSettings = configuration?.GetSection("EmailSettings");
            
            var isConfigured = !string.IsNullOrEmpty(emailSettings?["SmtpServer"]) &&
                              !string.IsNullOrEmpty(emailSettings?["SenderEmail"]);

            return Ok(new
            {
                isConfigured,
                smtpServer = emailSettings?["SmtpServer"],
                senderEmail = emailSettings?["SenderEmail"]
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking email status");
            return StatusCode(500, new { message = "Đã xảy ra lỗi khi kiểm tra trạng thái email" });
        }
    }
}

