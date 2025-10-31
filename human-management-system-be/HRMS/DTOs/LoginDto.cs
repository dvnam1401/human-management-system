using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Username hoặc Email là bắt buộc")]
    public string UsernameOrEmail { get; set; } = null!;

    [Required(ErrorMessage = "Password là bắt buộc")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; } = false;
}

