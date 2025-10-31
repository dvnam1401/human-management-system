using System;
using System.Collections.Generic;

namespace HRMS.Model1s;

public partial class AuditLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Action { get; set; }

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? User { get; set; }
}
