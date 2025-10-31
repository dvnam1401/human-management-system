using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class EmployeeAimatching
{
    public int Id { get; set; }

    public string? RequirementDetails { get; set; }

    public int? CreatedById { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? CreatedBy { get; set; }

    public virtual ICollection<EmployeeMatchingOutput> EmployeeMatchingOutputs { get; set; } = new List<EmployeeMatchingOutput>();
}
