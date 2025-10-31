namespace HRMS.DTOs;

public class LoginResponseDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
}

