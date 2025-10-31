using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class SendEmailDto
{
    [Required(ErrorMessage = "Email người nhận là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string RecipientEmail { get; set; } = null!;

    [Required(ErrorMessage = "Tiêu đề email là bắt buộc")]
    public string Subject { get; set; } = null!;

    [Required(ErrorMessage = "Nội dung email là bắt buộc")]
    public string Body { get; set; } = null!;
}

