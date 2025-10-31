using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class Employee
{
    public int Id { get; set; }

    public int? DepartmentId { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateOnly? TerminationDate { get; set; }

    public decimal? Salary { get; set; }

    public int? ManagerId { get; set; }

    public string? Address { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<EmployeeMatchingOutput> EmployeeMatchingOutputs { get; set; } = new List<EmployeeMatchingOutput>();

    public virtual User IdNavigation { get; set; } = null!;

    public virtual User? Manager { get; set; }
}
