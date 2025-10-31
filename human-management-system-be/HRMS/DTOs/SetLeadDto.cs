using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs;

public class SetLeadDto
{
    [Required(ErrorMessage = "HeadOfDepartmentId is required")]
    public int HeadOfDepartmentId { get; set; }
}

