using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog.Formatting.Compact;
using Serilog;
using System.Reflection;
using DiscountService.API.Application.Dtos;
using DiscountService.API.Utils.Filters;
using DiscountService.API.Utils.ExceptionHandlers;
using DiscountService.API.Utils;
using Microsoft.EntityFrameworkCore;
using DiscountService.API.Infrastructure;
using DiscountService.API.Application.Services;
using EventBus.RabbitMQ;
using DiscountService.API.Application.IntegrationEventHandlers.Course;
using DiscountService.API.Application.IntegrationEvents.Course;
using Microsoft.AspNetCore.Authorization;
using DiscountService.API.Utils.Authorization.Handlers;
using DiscountService.API.Utils.Authorization;
using DiscountService.API.Infrastructure.Cache;
using StackExchange.Redis;

namespace DiscountService.API.Extensions
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
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("GetCouponsByCoursePolicy", policy =>
                    policy.Requirements.Add(new GetCouponsByCourseRequirement()));
            });

            builder.Services.AddScoped<IAuthorizationHandler, GetCouponsByCourseAuthorizationHandler>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.AddHealthChecks();

            builder.AddEventBus();

            builder.AddDb();
            builder.AddRedis();

            builder.Services.AddScoped<ICouponCodeGenerator, CouponCodeGenerator>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<ICouponCachedRepository, CouponCachedRepository>();
            builder.Services.AddScoped<ICourseCachedRepository, CourseCachedRepository>();
        }

        private static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddMySql(connectionString: builder.Configuration["ConnectionStrings:DiscountDB"] ?? "")
                .AddRedis(builder.Configuration["ConnectionStrings:Redis"] ?? "")
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
                    "logs/discount-service.log",
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
                    Title = "Vicourses Discount API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Discount Service"
                });

                c.DocumentFilter<SecurityDefinitionDocumentFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static void AddDb(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration["ConnectionStrings:DiscountDB"] ?? "";
            var isDevelopment = builder.Environment.EnvironmentName == "Development";

            builder.Services.AddDbContext<DiscountServiceDbContext>(opt =>
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
            var redisConnStr = builder.Configuration["ConnectionStrings:Redis"] ?? "";

            builder.Services.AddSingleton<IConnectionMultiplexer>(s => ConnectionMultiplexer.Connect(redisConnStr));
        }

        private static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddRabbitMQEventBus(c =>
            {
                c.UriString = builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "";

                c.ConfigureConsume<CourseInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.info.updated";
                    opt.QueueOptions.QueueName = "discount_service_course.info.updated";
                });
                c.ConfigureConsume<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                    opt.QueueOptions.QueueName = "discount_service_course.published";
                });
            })
            .AddIntegrationEventHandler<CourseInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CoursePublishedIntegrationEventHandler>();
        }
    }
}
