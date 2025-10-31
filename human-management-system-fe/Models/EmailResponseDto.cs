namespace human_management_system_fe.Models;

public class EmailResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? SentAt { get; set; }
}

