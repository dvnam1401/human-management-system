namespace HRMS.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public int? HeadOfDepartmentId { get; set; }
    public string? HeadOfDepartmentName { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int EmployeeCount { get; set; }
}

