using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class VerifyOtpDto
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "OTP là bắt buộc")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP phải có 6 ký tự")]
    public string Otp { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public string NewPassword { get; set; } = null!;
}

