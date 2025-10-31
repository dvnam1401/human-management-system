using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<EmployeeAimatching> EmployeeAimatchings { get; set; } = new List<EmployeeAimatching>();

    public virtual Employee? EmployeeIdNavigation { get; set; }

    public virtual ICollection<Employee> EmployeeManagers { get; set; } = new List<Employee>();
}
