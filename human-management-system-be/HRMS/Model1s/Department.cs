using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public int? HeadOfDepartmentId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual User? HeadOfDepartment { get; set; }

    public virtual ICollection<RecruitmentRequest> RecruitmentRequests { get; set; } = new List<RecruitmentRequest>();
}
