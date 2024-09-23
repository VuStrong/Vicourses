using CourseService.Application.IntegrationEventHandlers.User;
using CourseService.Application.IntegrationEvents.User;
using CourseService.Application.Services;
using CourseService.EventBus;
using CourseService.EventBus.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace CourseService.Application
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File(
                    new CompactJsonFormatter(),
                    "logs/course-service.log",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 2242880,
                    retainedFileCountLimit: 2)
                .CreateLogger();
            services.AddSerilog();

            services.AddScoped<ICourseService, Services.CourseService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseCurriculumService, CourseCurriculumService>();
        }

        public static void AddEventBus(this IServiceCollection services, string uri)
        {
            services.AddSingleton<IEventBus, RabbitMQEventBus>(s =>
            {
                var scopeFactory = s.GetRequiredService<IServiceScopeFactory>();
                var logger = s.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMQEventBus>>();
                return new RabbitMQEventBus(uri, scopeFactory, logger);
            });

            services.AddScoped<UserCreatedIntegrationEventHandler>();
        }

        public static void ConfigureEventBus(this IHost app)
        {
            var eventBus = app.Services.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>();
        }
    }
}
