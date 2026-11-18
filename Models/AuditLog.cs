namespace NhsTriageAuditApi.Models
{
    public class AuditLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? ResourceAffected { get; set; }
        public string? Details { get; set; }
        public string? IpAddress { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}