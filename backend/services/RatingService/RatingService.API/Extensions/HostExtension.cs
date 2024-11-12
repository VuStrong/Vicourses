using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Compact;
using Serilog;
using System.Reflection;
using EventBus.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Utils;
using RatingService.API.Utils.ExceptionHandlers;
using RatingService.API.Utils.Filters;
using RatingService.API.Infrastructure;
using RatingService.API.Application.Dtos;
using RatingService.API.Application.IntegrationEvents.User;
using RatingService.API.Application.IntegrationEvents.Course;
using RatingService.API.Application.IntegrationEventHandlers.User;
using RatingService.API.Application.IntegrationEventHandlers.Course;
using RatingService.API.Utils.Authorization;
using Microsoft.AspNetCore.Authorization;
using RatingService.API.Utils.Authorization.Handlers;
using RatingService.API.Application.Services;
using RatingService.API.Application.IntegrationEvents.Rating;

namespace RatingService.API.Extensions
{
    public static class HostExtension
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
                });

            builder.AddSwagger();
            builder.AddLogger();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("GetRatingsByCourseIdPolicy", policy => 
                    policy.Requirements.Add(new GetRatingsByCourseIdRequirement()));
            });

            builder.Services.AddScoped<IAuthorizationHandler, GetRatingsByCourseIdAuthorizationHandler>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.AddHealthChecks();

            builder.Services.AddScoped<IRatingService, Application.Services.RatingService>();

            builder.AddEventBus();

            builder.AddDb();
        }

        private static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddMySql(connectionString: builder.Configuration["DB_CONNECTION_STRING"] ?? "")
                .AddRabbitMQ(rabbitConnectionString: builder.Configuration["RABBITMQ_URI"] ?? "");
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
                    "logs/rating-service.log",
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
                    Title = "Vicourses Rating API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Rating Service"
                });

                c.DocumentFilter<SecurityDefinitionDocumentFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static void AddDb(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["DB_CONNECTION_STRING"] ?? "";
            var isDevelopment = builder.Environment.EnvironmentName == "Development";

            builder.Services.AddDbContext<RatingServiceDbContext>(opt =>
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
                c.UriString = builder.Configuration["RABBITMQ_URI"] ?? "";

                c.ConfigurePublish<CourseRatingUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.rating.updated";
                });

                c.ConfigureConsume<UserCreatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.created";
                    opt.QueueOptions.QueueName = "rating_service_user.created";
                });
                c.ConfigureConsume<UserInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.info.updated";
                    opt.QueueOptions.QueueName = "rating_service_user.info.updated";
                });

                c.ConfigureConsume<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                    opt.QueueOptions.QueueName = "rating_service_course.published";
                });
                c.ConfigureConsume<CourseUnpublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.unpublished";
                    opt.QueueOptions.QueueName = "rating_service_course.unpublished";
                });
                c.ConfigureConsume<UserEnrolledIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.enrolled";
                    opt.QueueOptions.QueueName = "rating_service_user.enrolled";
                });
            })
            .AddIntegrationEventHandler<UserCreatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CoursePublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CourseUnpublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserEnrolledIntegrationEventHandler>();
        }
    }
}
