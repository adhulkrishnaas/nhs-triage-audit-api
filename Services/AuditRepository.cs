using Dapper;
using MySqlConnector;
using NhsTriageAuditApi.Models;

namespace NhsTriageAuditApi.Services
{
    public interface IAuditRepository
    {
        Task LogActionAsync(AuditLog auditLog);
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly IConfiguration _configuration;

        public AuditRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task LogActionAsync(AuditLog auditLog)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);

            const string sql = @"
                INSERT INTO AuditLogs (Id, Timestamp, UserId, UserEmail, Action, ResourceAffected, Details, IpAddress, StatusCode, IsSuccess)
                VALUES (@Id, @Timestamp, @UserId, @UserEmail, @Action, @ResourceAffected, @Details, @IpAddress, @StatusCode, @IsSuccess);";

            await connection.ExecuteAsync(sql, auditLog);
        }
    }
}