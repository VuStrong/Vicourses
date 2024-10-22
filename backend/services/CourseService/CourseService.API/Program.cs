using CourseService.API.Extensions;
using CourseService.Application;
using CourseService.Infrastructure;
using HealthChecks.UI.Client;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();

builder.Services.AddApplicationServices(c =>
{
    c.RabbitMQUri = builder.Configuration["RABBITMQ_URI"] ?? "";
    c.FileUploadSecret = builder.Configuration["FILE_UPLOAD_SECRET"] ?? "";
});

builder.Services.AddInfrastructureServices(c =>
{
    c.DbConnectionString = builder.Configuration["DATABASE_URL"] ?? "";
    c.DbName = builder.Configuration["DATABASE_NAME"] ?? "";
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course API - v1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new() {
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseExceptionHandler(opt => { });

_ = app.SeedDatabase();

app.Run();
