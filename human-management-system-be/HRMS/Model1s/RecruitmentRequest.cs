using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class RecruitmentRequest
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Position { get; set; }

    public string? Level { get; set; }

    public int? NumberOfPositions { get; set; }

    public string? Skills { get; set; }

    public string? Experience { get; set; }

    public int? DepartmentId { get; set; }

    public string? Status { get; set; }

    public string? TrackingProgressStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<RecruitmentPlan> RecruitmentPlans { get; set; } = new List<RecruitmentPlan>();
}
