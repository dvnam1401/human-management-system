namespace HRMS.DTOs;

public class EmailResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public DateTime? SentAt { get; set; }
}

