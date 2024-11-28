using CourseService.API.Extensions;
using CourseService.Application;
using CourseService.Infrastructure;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();

builder.Services.AddApplicationServices(c =>
{
    c.RabbitMQUri = builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "";
    c.RabbitMQRetryDelay = int.Parse(builder.Configuration["RabbitMqRetryDelay"] ?? "0");
    c.FileUploadSecret = builder.Configuration["FileUploadSecret"] ?? "";
});

builder.Services.AddInfrastructureServices(c =>
{
    c.DbConnectionString = builder.Configuration["CourseDB:Uri"] ?? "";
    c.DbName = builder.Configuration["CourseDB:DbName"] ?? "";
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
