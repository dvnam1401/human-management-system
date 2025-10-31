using System.ComponentModel.DataAnnotations;

namespace human_management_system_fe.Models;

public class LoginDto
{
    [Required(ErrorMessage = "Username hoặc Email là bắt buộc")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password là bắt buộc")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

