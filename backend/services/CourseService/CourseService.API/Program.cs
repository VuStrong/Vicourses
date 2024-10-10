using CourseService.API.Extensions;
using CourseService.Infrastructure;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

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

app.UseExceptionHandler(opt => { });

_ = app.SeedDatabase();

app.Run();
