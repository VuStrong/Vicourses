using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog.Formatting.Compact;
using Serilog;
using System.Reflection;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.IntegrationEventHandlers.Course;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Application.Services;
using WishlistService.API.Infrastructure.ClassMaps;
using WishlistService.API.Infrastructure.CollectionSeeders;
using WishlistService.API.Models;
using WishlistService.API.Utils;
using WishlistService.API.Utils.ExceptionHandlers;
using WishlistService.API.Utils.Filters;
using WishlistService.API.Infrastructure.Repositories;

namespace WishlistService.API.Extensions
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

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.AddHealthChecks();

            builder.AddEventBus();

            builder.AddDb();

            builder.Services.AddScoped<IWishlistService, Application.Services.WishlistService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
        }

        private static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddMongoDb(mongodbConnectionString: builder.Configuration["WishlistDB:Uri"] ?? "")
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
                    "logs/wishlist-service.log",
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
                    Title = "Vicourses Wishlist API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Wishlist Service"
                });

                c.DocumentFilter<SecurityDefinitionDocumentFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddRabbitMQEventBus(c =>
            {
                c.UriString = builder.Configuration["ConnectionStrings:RabbitMQ"] ?? "";
                c.RetryDelay = int.Parse(builder.Configuration["RabbitMqRetryDelay"] ?? "0");

                c.ConfigureConsume<CourseInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.info.updated";
                    opt.QueueOptions.QueueName = "wishlist_service_course.info.updated";
                });
                c.ConfigureConsume<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                    opt.QueueOptions.QueueName = "wishlist_service_course.published";
                });
                c.ConfigureConsume<CourseUnpublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.unpublished";
                    opt.QueueOptions.QueueName = "wishlist_service_course.unpublished";
                });
                c.ConfigureConsume<UserEnrolledIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.enrolled";
                    opt.QueueOptions.QueueName = "wishlist_service_user.enrolled";
                });
                c.ConfigureConsume<UserUnenrolledIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "user.unenrolled";
                    opt.QueueOptions.QueueName = "wishlist_service_user.unenrolled";
                });
            })
            .AddIntegrationEventHandler<CourseInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CoursePublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CourseUnpublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserEnrolledIntegrationEventHandler>()
            .AddIntegrationEventHandler<UserUnenrolledIntegrationEventHandler>();
        }

        private static void AddDb(this WebApplicationBuilder builder)
        {
            CourseMap.Configure();
            WishlistMap.Configure();

            var connectionString = builder.Configuration["WishlistDB:Uri"] ?? "";
            var dbName = builder.Configuration["WishlistDB:DbName"] ?? "";

            builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

            builder.Services.AddScoped<IMongoCollection<Course>>(s =>
            {
                var client = s.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(dbName);
                return database.GetCollection<Course>("courses");
            });
            builder.Services.AddScoped<IMongoCollection<Wishlist>>(s =>
            {
                var client = s.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(dbName);
                return database.GetCollection<Wishlist>("wishlists");
            });

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
        }

        public static async Task SeedDatabase(this IHost app)
        {
            await app.SeedMongoCollections();
        }
    }
}
