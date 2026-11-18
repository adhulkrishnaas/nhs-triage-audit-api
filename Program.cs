using NhsTriageAuditApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add CORS policy for your frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Replace with your frontend URL if different
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NHS Triage Audit API v1");
    c.RoutePrefix = string.Empty;
});

// 2. Enable CORS middleware (Must be before MapControllers)
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();

app.Run();