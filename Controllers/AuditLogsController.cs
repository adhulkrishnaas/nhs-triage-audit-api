using Microsoft.AspNetCore.Mvc;
using NhsTriageAuditApi.Models;
using NhsTriageAuditApi.Services;

namespace NhsTriageAuditApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditRepository _auditRepository;

        public AuditLogsController(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuditLog([FromBody] AuditLog auditLog)
        {
            try
            {
                if (string.IsNullOrEmpty(auditLog.UserId) || string.IsNullOrEmpty(auditLog.Action))
                {
                    return BadRequest(new { message = "UserId and Action are required fields for clinical audit compliance." });
                }

                // Capture client IP address securely
                auditLog.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

                await _auditRepository.LogActionAsync(auditLog);

                return StatusCode(201, new { message = "Audit log recorded successfully.", logId = auditLog.Id });
            }
            catch (Exception ex)
            {
                // Return the exact database or Dapper error for debugging
                return StatusCode(500, new { error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }
    }
}