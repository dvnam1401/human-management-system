using System.ComponentModel.DataAnnotations;

namespace human_management_system_fe.Models;

public class SendEmailDto
{
    [Required(ErrorMessage = "Email người nhận là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string RecipientEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nội dung là bắt buộc")]
    public string Body { get; set; } = string.Empty;
}

