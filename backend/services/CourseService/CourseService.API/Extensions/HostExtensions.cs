using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CourseService.API.Utils;
using Microsoft.AspNetCore.Mvc;
using CourseService.Application.Dtos;
using CourseService.API.Utils.ExceptionHandlers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using CourseService.API.Utils.Authorization.Handlers;
using CourseService.API.Utils.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog.Formatting.Compact;
using Serilog;

namespace CourseService.API.Extensions
{
    public static class HostExtensions
    {
        public static void AddApiServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(o => {
                    o.InvalidModelStateResponseFactory = context => {
                        var errors = context.GetModelStateErrorMessages();
                        var problems = new ValidationErrorResponseDto(errors);

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

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("GetCoursePolicy", policy => 
                    policy.Requirements.Add(new GetCourseRequirement()));

                opt.AddPolicy("GetSectionPolicy", policy =>
                    policy.Requirements.Add(new GetSectionRequirement()));
                
                opt.AddPolicy("GetLessonPolicy", policy =>
                    policy.Requirements.Add(new GetLessonRequirement()));

                opt.AddPolicy("GetEnrolledCoursesPolicy", policy =>
                    policy.Requirements.Add(new GetEnrolledCoursesRequirement()));
            });

            builder.Services.AddScoped<IAuthorizationHandler, GetCourseAuthorizationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, GetSectionAuthorizationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, GetLessonAuthorizationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, GetEnrolledCoursesAuthorizationHandler>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.AddHealthChecks();
        }

        private static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddMongoDb(mongodbConnectionString: builder.Configuration["CourseDB:Uri"] ?? "")
                .AddRabbitMQ(rabbitConnectionString: builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "");
        }

        public static void AddLogger(this WebApplicationBuilder builder)
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
                    "logs/course-service.log",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 2242880,
                    retainedFileCountLimit: 2)
                .CreateLogger();

            builder.Services.AddSerilog();
        }

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vicourses Courses API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Course Service"
                });

                c.DocumentFilter<SecurityDefinitionDocumentFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}