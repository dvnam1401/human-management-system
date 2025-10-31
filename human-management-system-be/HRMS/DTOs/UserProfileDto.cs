namespace HRMS.DTOs;

public class UserProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
    
    // Thông tin Employee nếu user là employee
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public string? Address { get; set; }
}

