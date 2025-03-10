﻿using CourseService.Application.DomainEventHandlers.Course;
using CourseService.Application.DomainEventHandlers.Enrollment;
using CourseService.Application.DomainEventHandlers.Lesson;
using CourseService.Application.IntegrationEventHandlers.Payment;
using CourseService.Application.IntegrationEventHandlers.Rating;
using CourseService.Application.IntegrationEventHandlers.User;
using CourseService.Application.IntegrationEventHandlers.VideoProcessing;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Application.IntegrationEvents.Email;
using CourseService.Application.IntegrationEvents.Payment;
using CourseService.Application.IntegrationEvents.Rating;
using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Application.IntegrationEvents.User;
using CourseService.Application.IntegrationEvents.VideoProcessing;
using CourseService.Application.Interfaces;
using CourseService.Application.Services;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Events.Enrollment;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Services;
using CourseService.Domain.Services.Implementations;
using EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace CourseService.Application
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IServiceCollection services, Action<ApplicationConfiguration>? config = null)
        {
            var appConfiguration = new ApplicationConfiguration();
            config?.Invoke(appConfiguration);

            services.AddSingleton(appConfiguration);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // App services
            services.AddScoped<ICourseService, Services.CourseService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseCurriculumService, CourseCurriculumService>();
            services.AddScoped<IEnrollService, EnrollService>();
            services.AddScoped<ICommentService, CommentService>();

            // Domain services
            services.AddScoped<ICategoryDomainService, CategoryDomainService>();
            services.AddScoped<ICommentDomainService, CommentDomainService>();
            services.AddScoped<IDeleteResourceDomainService, DeleteResourceDomainService>();

            services.AddDomainEventHandlers();            

            services.AddEventBus(appConfiguration);

            services.AddScoped<IFileUploadTokenValidator, FileUploadTokenValidator>();
        }

        private static void AddDomainEventHandler<T, TH>(this IServiceCollection services) 
            where T : DomainEvent 
            where TH : class, IDomainEventHandler<T>
        {
            services.AddKeyedScoped<IDomainEventHandler, TH>(typeof(T));
        }

        private static void AddDomainEventHandlers(this IServiceCollection services)
        {
            services.AddDomainEventHandler<CoursePublishedDomainEvent, CoursePublishedDomainEventHandler>();
            services.AddDomainEventHandler<CourseUnpublishedDomainEvent, CourseUnpublishedDomainEventHandler>();
            services.AddDomainEventHandler<CourseInfoUpdatedDomainEvent, CourseInfoUpdatedDomainEventHandler>();
            services.AddDomainEventHandler<CourseDeletedDomainEvent, CourseDeletedDomainEventHandler>();
            services.AddDomainEventHandler<CoursePreviewVideoUpdatedDomainEvent, CoursePreviewVideoUpdatedDomainEventHandler>();
            services.AddDomainEventHandler<CourseThumbnailUpdatedDomainEvent, CourseThumbnailUpdatedDomainEventHandler>();
            services.AddDomainEventHandler<CourseApprovedDomainEvent, CourseApprovedDomainEventHandler>();
            services.AddDomainEventHandler<CourseApprovalCanceledDomainEvent, CourseApprovalCanceledDomainEventHandler>();

            services.AddDomainEventHandler<LessonDeletedDomainEvent, LessonDeletedDomainEventHandler>();
            services.AddDomainEventHandler<LessonVideoUpdatedDomainEvent, LessonVideoUpdatedDomainEventHandler>();

            services.AddDomainEventHandler<UserEnrolledDomainEvent, UserEnrolledDomainEventHandler>();
            services.AddDomainEventHandler<UserUnenrolledDomainEvent, UserUnenrolledDomainEventHandler>();
        }

        private static void AddEventBus(this IServiceCollection services, ApplicationConfiguration configuration)
        {
            services.AddRabbitMQEventBus(c =>
            {
                c.UriString = configuration.RabbitMQUri;
                c.RetryDelay = configuration.RabbitMQRetryDelay;

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
                c.ConfigurePublish<UserEnrolledIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.enrolled";
                });
                c.ConfigurePublish<UserUnenrolledIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.unenrolled";
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

                // Events in Rating Service
                c.ConfigureConsume<CourseRatingUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.rating.updated";
                    opt.QueueOptions.QueueName = "course_service_course.rating.updated";
                });

                // Events in Email Service
                c.ConfigurePublish<SendEmailIntegrationEvent>(opt =>
                {
                    opt.ExcludeExchange = true;
                    opt.RoutingKey = "send_email";
                });

                // Events in Payment Service
                c.ConfigureConsume<PaymentCompletedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "payment.completed";
                    opt.QueueOptions.QueueName = "course_service_payment.completed";
                });
                c.ConfigureConsume<PaymentRefundedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "payment.refunded";
                    opt.QueueOptions.QueueName = "course_service_payment.refunded";
                });
            })
            .AddIntegrationEventHandler<UserCreatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<VideoProcessingCompletedIntegrationEventHandler>()
            .AddIntegrationEventHandler<VideoProcessingFailedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CourseRatingUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<PaymentCompletedIntegrationEventHandler>()
            .AddIntegrationEventHandler<PaymentRefundedIntegrationEventHandler>();
        }
    }

    public class ApplicationConfiguration
    {
        public string RabbitMQUri { get; set; } = string.Empty;
        public int RabbitMQRetryDelay { get; set; } = 0;

        public string FileUploadSecret { get; set; } = string.Empty;
        public string MediaFileSecret { get; set; } = string.Empty;
    }
}
