﻿using CourseService.Application.DomainEventHandlers.Course;
using CourseService.Application.DomainEventHandlers.Lession;
using CourseService.Application.IntegrationEventHandlers.User;
using CourseService.Application.IntegrationEvents.User;
using CourseService.Application.Interfaces;
using CourseService.Application.Services;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Events.Lession;
using CourseService.Domain.Services;
using CourseService.Domain.Services.Implementations;
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

            // Logger
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

            // App services
            services.AddScoped<ICourseService, Services.CourseService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseCurriculumService, CourseCurriculumService>();
            services.AddScoped<ILessionQuizService, LessionQuizService>();

            // Domain services
            services.AddScoped<IQuizDomainService, QuizDomainService>();
            services.AddScoped<ICategoryDomainService, CategoryDomainService>();

            // Domain event handlers
            services.AddScoped<IDomainEventHandler<CoursePublishedDomainEvent>, CoursePublishedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseUnpublishedDomainEvent>, CourseUnpublishedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseInfoUpdatedDomainEvent>, CourseInfoUpdatedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<LessionDeletedDomainEvent>, LessionDeletedDomainEventHandler>();
        }

        public static void AddEventBus(this IServiceCollection services, string uri)
        {
            services.AddSingleton<IEventBus, RabbitMQEventBus>(s =>
            {
                var scopeFactory = s.GetRequiredService<IServiceScopeFactory>();
                var logger = s.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMQEventBus>>();
                return new RabbitMQEventBus(uri, scopeFactory, logger)
                {
                    ServiceName = "course_service"
                };
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
