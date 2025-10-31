using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Code is required")]
    [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters")]
    public string Code { get; set; } = null!;

    [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
    public string? Description { get; set; }

    public int? HeadOfDepartmentId { get; set; }
}

