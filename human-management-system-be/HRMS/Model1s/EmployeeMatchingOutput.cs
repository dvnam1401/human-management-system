using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class EmployeeMatchingOutput
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? EmployeeAimatchingId { get; set; }

    public double? MatchScore { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual EmployeeAimatching? EmployeeAimatching { get; set; }
}
