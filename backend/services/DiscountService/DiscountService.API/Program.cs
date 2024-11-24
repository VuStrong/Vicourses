using DiscountService.API.Extensions;
using DiscountService.API.GrpcServices;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount API - v1");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<DiscountGrpcService>();

app.MapHealthChecks("/hc", new()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseExceptionHandler(opt => { });

app.Run();

public partial class Program { }