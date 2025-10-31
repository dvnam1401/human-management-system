namespace HRMS.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateOnly? TerminationDate { get; set; }
    public decimal? Salary { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public string? Address { get; set; }
    public bool? IsActive { get; set; }
}

