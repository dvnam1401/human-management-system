using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class CreateRegisterDto
{
    [Required(ErrorMessage = "Username là bắt buộc")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 50 ký tự")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password là bắt buộc")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password phải có ít nhất 6 ký tự")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Xác nhận password là bắt buộc")]
    [Compare("Password", ErrorMessage = "Password và xác nhận password không khớp")]
    public string ConfirmPassword { get; set; } = null!;

    [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
    public string? FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
    public string? LastName { get; set; }

    [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }

    [StringLength(20, ErrorMessage = "Role không được vượt quá 20 ký tự")]
    public string? Role { get; set; }
}

