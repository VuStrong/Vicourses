using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Config ocelot
builder.Configuration
    .AddJsonFile("ocelot.json")
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json");

builder.Services.AddOcelot();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Map("/swagger/v1/swagger-auth.json", b =>
{
    b.Run(async x => {
        var json = File.ReadAllText("swagger/swagger-auth.json");
        await x.Response.WriteAsync(json);
    });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger-auth.json", "Auth");
});

app.UseHttpsRedirection();

app.UseOcelot().Wait();

app.Run();
