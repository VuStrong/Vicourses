using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Config ocelot
builder.Configuration
    .AddJsonFile("ocelot.json")
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json");

builder.Services.AddOcelot();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors-policy", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapServiceSwaggers(builder.Configuration);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger-auth.json", "Auth API - v1");
    c.SwaggerEndpoint("/swagger/v1/swagger-course.json", "Course API - v1");
    c.SwaggerEndpoint("/swagger/v1/swagger-storage.json", "Storage API - v1");
    c.SwaggerEndpoint("/swagger/v1/swagger-search.json", "Search API - v1");
});

app.UseHttpsRedirection();

app.UseCors("cors-policy");

app.UseHealthChecksUI(options => {
    options.UIPath = "/hc-ui";
});

app.UseOcelot().Wait();

app.Run();
