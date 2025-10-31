using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class RecruitmentPlan
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Position { get; set; }

    public string? Level { get; set; }

    public int? NumberOfPositions { get; set; }

    public string? Skills { get; set; }

    public string? Experience { get; set; }

    public string? JobPostDetails { get; set; }

    public decimal? SalaryRangeMin { get; set; }

    public decimal? SalaryRangeMax { get; set; }

    public string? Status { get; set; }

    public string? TrackingProgressStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RecruitmentRequest> RecruitmentRequests { get; set; } = new List<RecruitmentRequest>();
}
