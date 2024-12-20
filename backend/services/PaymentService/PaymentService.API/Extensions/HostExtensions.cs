﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PaymentService.API.Utils.Filters;
using Serilog.Formatting.Compact;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PaymentService.API.Utils;
using PaymentService.API.Utils.ExceptionHandlers;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Infrastructure;
using EventBus.RabbitMQ;
using PaymentService.API.Application.Configurations;
using PaymentService.API.Application.Services;
using PaymentService.API.Application.Services.Paypal;
using PaymentService.API.Application.IntegrationEvents;
using System.Text.Json.Serialization;
using PaymentService.API.Responses;
using PaymentService.API.Application.BackgroundServices;
using PaymentService.API.Application.IntegrationEvents.Course;
using PaymentService.API.Application.IntegrationEvents.User;
using PaymentService.API.Application.IntegrationEventHandlers.Course;
using PaymentService.API.Application.IntegrationEventHandlers.User;
using Quartz;

namespace PaymentService.API.Extensions
{
    public static class HostExtensions
    {
        public static void AddApiServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(o =>
                {
                    o.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.GetModelStateErrorMessages();
                        var problems = new ValidationErrorResponse(errors);

                        return new BadRequestObjectResult(problems);
                    };
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.AddSwagger();
            builder.AddLogger();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = SecurityHelper.GetRsaSecurityKeyFromFile("public.key");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    RequireSignedTokens = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = key,
                };
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.AddHealthChecks();

            builder.AddEventBus();

            builder.AddDb();

            builder.AddConfigurations();

            builder.AddApplicationServices();

            builder.AddBackgroundJobs();
        }

        private static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddMySql(connectionString: builder.Configuration["ConnectionStrings:PaymentDB"] ?? "")
                .AddRabbitMQ(rabbitConnectionString: builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "");
        }

        private static void AddLogger(this WebApplicationBuilder builder)
        {
            var loggerConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(loggerConfiguration)
                .WriteTo.Console()
                .WriteTo.File(
                    new CompactJsonFormatter(),
                    "logs/payment-service.log",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 2242880,
                    retainedFileCountLimit: 2)
                .CreateLogger();

            builder.Services.AddSerilog();
        }

        private static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vicourses Payment API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Payment Service"
                });

                c.DocumentFilter<SecurityDefinitionDocumentFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static void AddDb(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["ConnectionStrings:PaymentDB"] ?? "";
            var isDevelopment = builder.Environment.EnvironmentName == "Development";

            builder.Services.AddDbContext<PaymentServiceDbContext>(opt =>
            {
                var builder = opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                if (isDevelopment)
                {
                    builder.LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                }
            });
        }

        private static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddRabbitMQEventBus(c =>
            {
                c.UriString = builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "";
                c.RetryDelay = int.Parse(builder.Configuration["RabbitMqRetryDelay"] ?? "0");

                c.ConfigurePublish<SendEmailIntegrationEvent>(opt =>
                {
                    opt.ExcludeExchange = true;
                    opt.RoutingKey = "send_email";
                });

                c.ConfigurePublish<PaymentCompletedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "payment.completed";
                });
                c.ConfigurePublish<PaymentRefundedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "payment.refunded";
                });

                c.ConfigureConsume<CourseInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.info.updated";
                    opt.QueueOptions.QueueName = "payment_service_course.info.updated";
                });
                c.ConfigureConsume<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                    opt.QueueOptions.QueueName = "payment_service_course.published";
                });
                c.ConfigureConsume<CourseUnpublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.unpublished";
                    opt.QueueOptions.QueueName = "payment_service_course.unpublished";
                });

                c.ConfigureConsume<UserCreatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.created";
                    opt.QueueOptions.QueueName = "payment_service_user.created";
                });
                c.ConfigureConsume<UserInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.info.updated";
                    opt.QueueOptions.QueueName = "payment_service_user.info.updated";
                });
                c.ConfigureConsume<UserPaypalAccountUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.paypalaccount.updated";
                    opt.QueueOptions.QueueName = "payment_service_user.paypalaccount.updated";
                });
            })
            .AddIntegrationEventHandler<CourseInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CoursePublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CourseUnpublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserCreatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserPaypalAccountUpdatedIntegrationEventHandler>();
        }

        private static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<PaypalConfiguration>(c =>
            {
                c.ClientId = builder.Configuration["Paypal:ClientId"] ?? "";
                c.ClientSecret = builder.Configuration["Paypal:ClientSecret"] ?? "";
                c.Base = builder.Configuration["Paypal:Base"] ?? "";
                c.Mode = builder.Configuration["Paypal:Mode"] ?? "";
            });
        }

        private static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPaymentService, Application.Services.PaymentService>();
            builder.Services.AddScoped<IPayoutService, Application.Services.PayoutService>();
            builder.Services.AddScoped<IDiscountService, DiscountGrpcService>(s =>
            {
                return new DiscountGrpcService(builder.Configuration["DiscountGrpcAddress"] ?? "");
            });
            builder.Services.AddScoped<IPaypalOrdersService, PaypalOrdersService>();
            builder.Services.AddScoped<IPaypalPaymentsService, PaypalPaymentsService>();
            builder.Services.AddScoped<IPaypalPayoutsService, PaypalPayoutsService>();
        }

        private static void AddBackgroundJobs(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<PaymentCancellationService>();

            builder.Services.AddQuartz(q =>
            {
                q.AddJob<WeeklyPayoutsJob>(opt => opt.WithIdentity("WeeklyPayoutsJob"));
                q.AddTrigger(opts => opts
                    .ForJob("WeeklyPayoutsJob")
                    .WithIdentity("WeeklyPayoutsJobTrigger")
                    .WithCronSchedule("0 0 6 ? * SUN"));
                //.WithCronSchedule("0 0/1 * * * ?"));
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
