using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CourseService.API.Utils;
using Microsoft.AspNetCore.Mvc;
using CourseService.Application.Dtos;
using CourseService.Application;
using CourseService.Infrastructure;
using CourseService.API.Utils.ExceptionHandlers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using CourseService.API.Utils.Authorization.Handlers;
using CourseService.API.Utils.Authorization;
using Microsoft.IdentityModel.Tokens;
using CourseService.Application.Utils;

namespace CourseService.API.Extensions
{
    public static class HostExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(o => {
                    o.InvalidModelStateResponseFactory = context => {
                        var errors = GetModelStateErrorMessages(context);
                        var problems = new ValidationErrorResponseDto(errors);

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

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("GetCoursePolicy", policy => 
                    policy.Requirements.Add(new GetCourseRequirement()));

                opt.AddPolicy("GetSectionPolicy", policy =>
                    policy.Requirements.Add(new GetSectionRequirement()));

                opt.AddPolicy("GetLessonPolicy", policy =>
                    policy.Requirements.Add(new GetLessonRequirement()));
            });

            builder.Services.AddScoped<IAuthorizationHandler, GetCourseAuthorizationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, GetSectionAuthorizationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, GetLessonAuthorizationHandler>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddApplicationServices();
            builder.Services.AddEventBus(builder.Configuration["RABBITMQ_URI"] ?? "");
            builder.Services.AddScoped<FileUploadValidator>(s =>
            {
                return new FileUploadValidator(builder.Configuration["FILE_UPLOAD_SECRET"] ?? "");
            });

            builder.Services.AddDbContext(
                builder.Configuration["DATABASE_URL"] ?? "",
                builder.Configuration["DATABASE_NAME"] ?? "");
            builder.Services.AddInfrastructureServices();
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

        private static List<string> GetModelStateErrorMessages(ActionContext context)
        {
            var errors = new List<string>();

            foreach (var keyModelStatePair in context.ModelState)
            {
                if (keyModelStatePair.Value.Errors != null)
                {
                    foreach (var error in keyModelStatePair.Value.Errors)
                    {
                        if (error == null) continue;

                        errors.Add(error.ErrorMessage);
                    }
                }
            }

            return errors;
        }
    }
}