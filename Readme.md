# NHS Clinical Triage & Audit Compliance API

A secure, high-performance **.NET 10 Web API** backend built to provide immutable audit logging and information governance (IG) compliance for the NHS Clinical Triage platform. It securely processes and persists asynchronous clinician audit trails using **Dapper ORM** and **MySQL**, deployed live on **Railway**.

---

## 🚀 Architecture & Tech Stack

- **Framework:** .NET 10 Web API (C#)
- **Database Access:** Dapper (Micro-ORM for high-throughput, low-overhead SQL execution)
- **Database:** MySQL (Hosted on Railway via private networking)
- **Hosting & Deployment:** Railway (Dockerized container)
- **Security & Governance:** CORS policy restricted to live Firebase frontend domain, IP address capture, immutable timestamping, and fail-safe asynchronous error handling.

---

## 📂 Project Structure

```text
NhsTriageAuditApi/
│
├── Controllers/
│   └── AuditLogsController.cs     # API endpoints for audit log ingestion
├── Models/
│   └── AuditLog.cs                # Data transfer object and database entity model
├── Services/
│   └── IAuditRepository.cs        # Data repository interface & Dapper implementation
├── Program.cs                     # DI container, CORS configuration, and app startup
├── Dockerfile                     # Production container build instructions
└── appsettings.json               # Configuration defaults

🛠️ Getting Started Locally
Prerequisites
.NET 10 SDK installed on your machine.

MySQL (Local instance or Docker container).

1. Clone the Repository
Bash
git clone https://github.com/adhulkrishnaas/nhs-triage-audit-api
cd nhs-triage-audit-api
2. Configure Local Database
Run the following SQL script in your local MySQL instance to create the required audit database and table:

SQL
CREATE DATABASE IF NOT EXISTS NHSTriageAuditDB;
USE NHSTriageAuditDB;

CREATE TABLE IF NOT EXISTS AuditLogs (
    Id VARCHAR(36) PRIMARY KEY,
    Timestamp DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UserId VARCHAR(128) NOT NULL,
    UserEmail VARCHAR(256) NULL,
    Action VARCHAR(150) NOT NULL,
    ResourceAffected VARCHAR(100) NULL,
    Details TEXT NULL,
    IpAddress VARCHAR(45) NULL,
    StatusCode INT NOT NULL,
    IsSuccess TINYINT(1) NOT NULL DEFAULT 1
);

CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp DESC);
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
3. Configure appsettings.json
Update or create your local connection string in appsettings.json:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=NHSTriageAuditDB;Uid=root;Pwd=YOUR_LOCAL_PASSWORD;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
4. Run the Application
Bash
dotnet run
The API will start locally at http://localhost:5000. You can explore the interactive Swagger documentation at http://localhost:5000/swagger.

📡 API Endpoints
POST /api/AuditLogs
Records an immutable clinical audit trail entry. Designed never to block or throw exceptions that disrupt primary clinical workflows.

Request Body Example:

JSON
{
  "userId": "firebase-uid-12345",
  "userEmail": "clinician.name@nhs.net",
  "action": "CASE_REVIEWED",
  "resourceAffected": "PatientCase-9921",
  "details": "Reviewed triage summary and confirmed priority rating.",
  "statusCode": 200,
  "isSuccess": true
}
Response: 201 Created with generated log ID.

🚢 Deployment (Railway)
This application is containerized using a multi-stage Dockerfile and deployed automatically on Railway:

Connected to the GitHub repository for continuous deployment on push.

Linked to a companion Railway MySQL service using private internal networking (${{MySQL.MYSQLHOST}}).

Environment variables configured securely via Railway's dashboard variables manager (ConnectionStrings__DefaultConnection).

🔒 Information Governance & Security
Immutability: Audit records are write-once logs with indexed timestamps to ensure full accountability and auditability for NHS clinical reviews.

Separation of Concerns: Audit logging runs asynchronously and independently from patient data storage to prevent workflow interruption.

CORS Restrictions: Strictly configured to accept requests exclusively from authorized frontend domains (Firebase Hosting).
```
