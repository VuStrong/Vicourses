﻿using CourseService.Application.DomainEventHandlers.Course;
using CourseService.Application.DomainEventHandlers.Lesson;
using CourseService.Application.DomainEventHandlers.Section;
using CourseService.Application.IntegrationEventHandlers.User;
using CourseService.Application.IntegrationEventHandlers.VideoProcessing;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Application.IntegrationEvents.User;
using CourseService.Application.IntegrationEvents.VideoProcessing;
using CourseService.Application.Interfaces;
using CourseService.Application.Services;
using CourseService.Application.Utils;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Events.Section;
using CourseService.Domain.Services;
using CourseService.Domain.Services.Implementations;
using CourseService.EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;

namespace CourseService.Application
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IServiceCollection services, Action<ApplicationConfiguration>? config = null)
        {
            var appConfiguration = new ApplicationConfiguration();
            config?.Invoke(appConfiguration);

            var loggerConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(loggerConfiguration)
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
            services.AddScoped<ILessonQuizService, LessonQuizService>();

            // Domain services
            services.AddScoped<IQuizDomainService, QuizDomainService>();
            services.AddScoped<ICategoryDomainService, CategoryDomainService>();

            // Domain event handlers
            services.AddScoped<IDomainEventHandler<CoursePublishedDomainEvent>, CoursePublishedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseUnpublishedDomainEvent>, CourseUnpublishedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseInfoUpdatedDomainEvent>, CourseInfoUpdatedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseDeletedDomainEvent>, CourseDeletedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CoursePreviewVideoUpdatedDomainEvent>, CoursePreviewVideoUpdatedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseThumbnailUpdatedDomainEvent>, CourseThumbnailUpdatedDomainEventHandler>();

            services.AddScoped<IDomainEventHandler<LessonDeletedDomainEvent>, LessonDeletedDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<LessonVideoUpdatedDomainEvent>, LessonVideoUpdatedDomainEventHandler>();

            services.AddScoped<IDomainEventHandler<SectionDeletedDomainEvent>, SectionDeletedDomainEventHandler>();

            services.AddEventBus(appConfiguration.RabbitMQUri);

            services.AddScoped<FileUploadValidator>(s =>
            {
                return new FileUploadValidator(appConfiguration.FileUploadSecret);
            });
        }

        private static void AddEventBus(this IServiceCollection services, string uri)
        {
            services.AddRabbitMQEventBus(c =>
            {
                c.UriString = uri;

                // Events in course service (this)
                c.ConfigurePublish<CourseInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.info.updated";
                });
                c.ConfigurePublish<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                });
                c.ConfigurePublish<CourseUnpublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.unpublished";
                });

                // Events in storage service
                c.ConfigurePublish<DeleteFilesIntegrationEvent>(opt =>
                {
                    opt.ExcludeExchange = true;
                    opt.RoutingKey = "delete_files";
                });

                // Events in user service
                c.ConfigureConsume<UserCreatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.created";
                    opt.QueueOptions.QueueName = "course_service_user.created";
                });
                c.ConfigureConsume<UserInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.info.updated";
                    opt.QueueOptions.QueueName = "course_service_user.info.updated";
                });

                // Events in video processing service
                c.ConfigurePublish<RequestVideoProcessingIntegrationEvent>(opt =>
                {
                    opt.ExcludeExchange = true;
                    opt.RoutingKey = "process_video";
                });
                c.ConfigureConsume<VideoProcessingCompletedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "video-processing.completed";
                    opt.QueueOptions.QueueName = "course_service_video-processing.completed";
                });
                c.ConfigureConsume<VideoProcessingFailedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "video-processing.failed";
                    opt.QueueOptions.QueueName = "course_service_video-processing.failed";
                });
            })
            .AddIntegrationEventHandler<UserCreatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<VideoProcessingCompletedIntegrationEventHandler>()
            .AddIntegrationEventHandler<VideoProcessingFailedIntegrationEventHandler>();
        }
    }

    public class ApplicationConfiguration
    {
        public string RabbitMQUri { get; set; } = string.Empty;

        public string FileUploadSecret { get; set; } = string.Empty;
    }
}
