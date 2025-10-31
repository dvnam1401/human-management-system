using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class ResetPasswordRequestDto
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;
}

