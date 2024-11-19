using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StatisticsService.API.Application.IntegrationEventHandlers.Course;
using StatisticsService.API.Application.IntegrationEventHandlers.User;
using StatisticsService.API.Application.IntegrationEvents.Course;
using StatisticsService.API.Application.IntegrationEvents.User;
using StatisticsService.API.Application.Services;
using StatisticsService.API.Infrastructure;
using StatisticsService.API.Responses;
using StatisticsService.API.Utils;
using StatisticsService.API.Utils.ExceptionHandlers;
using StatisticsService.API.Utils.Filters;
using System.Reflection;
using System.Text.Json.Serialization;

namespace StatisticsService.API.Extensions
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
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.AddSwagger();

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

            builder.AddDb();

            builder.AddRedis();

            builder.AddEventBus();

            builder.Services.AddScoped<IInstructorPerformanceStatistician, InstructorPerformanceStatistician>();
            builder.Services.AddScoped<IAdminDashboardDataStatistician, AdminDashboardDataStatistician>();
        }

        private static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddMySql(connectionString: builder.Configuration["ConnectionStrings:StatisticsDB"] ?? "")
                .AddRabbitMQ(rabbitConnectionString: builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "")
                .AddRedis(builder.Configuration["ConnectionStrings:Redis"] ?? "");
        }

        private static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vicourses Statistics API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Statistics Service"
                });

                c.DocumentFilter<SecurityDefinitionDocumentFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static void AddDb(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["ConnectionStrings:StatisticsDB"] ?? "";
            var isDevelopment = builder.Environment.EnvironmentName == "Development";

            builder.Services.AddDbContext<StatisticsServiceDbContext>(opt =>
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

        private static void AddRedis(this WebApplicationBuilder builder)
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["ConnectionStrings:Redis"];
            });
        }

        private static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddRabbitMQEventBus(c =>
            {
                c.UriString = builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "";

                // Events from course service
                c.ConfigureConsume<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                    opt.QueueOptions.QueueName = "statistics_service_course.published";
                });
                c.ConfigureConsume<UserEnrolledIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.enrolled";
                    opt.QueueOptions.QueueName = "statistics_service_user.enrolled";
                });

                // Events from user service
                c.ConfigureConsume<UserCreatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.created";
                    opt.QueueOptions.QueueName = "statistics_service_user.created";
                });
                //c.ConfigureConsume<UserRoleUpdatedIntegrationEvent>(opt =>
                //{
                //    opt.ExchangeOptions.ExchangeName = "user.role.updated";
                //    opt.QueueOptions.QueueName = "statistics_service_user.role.updated";
                //});
            })
            .AddIntegrationEventHandler<CoursePublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserEnrolledIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserCreatedIntegrationEventHandler>();
        }
    }
}
