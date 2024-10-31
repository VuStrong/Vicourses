using EventBus.RabbitMQ;
using Microsoft.OpenApi.Models;
using SearchService.API.Application.IntegrationEventHandlers.Course;
using SearchService.API.Application.IntegrationEvents.Course;
using Serilog.Formatting.Compact;
using Serilog;
using System.Reflection;
using SearchService.API.Application.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using SearchService.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SearchService.API.Application.Dtos;
using SearchService.API.Utils.ExceptionHandlers;
using System.Text.Json.Serialization;

namespace SearchService.API.Extensions
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
            builder.AddLogger();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.AddElasticsearch();

            builder.AddEventBus();

            builder.Services.AddHealthChecks();
        }

        private static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vicourses Search API",
                    Version = "v1",
                    Description = "Vicourses API documentation for Search Service"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
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
                    "logs/search-service.log",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 2242880,
                    retainedFileCountLimit: 2)
                .CreateLogger();

            builder.Services.AddSerilog();
        }

        private static void AddElasticsearch(this WebApplicationBuilder builder)
        {
            var uri = builder.Configuration["ELASTICSEARCH_URI"] ?? "";
            var username = builder.Configuration["ELASTICSEARCH_USER"] ?? "";
            var password = builder.Configuration["ELASTICSEARCH_PASS"] ?? "";

            var settings = new ElasticsearchClientSettings(new Uri(uri))
                .ServerCertificateValidationCallback((sender, cert, chain, errors) => true)
                .Authentication(new BasicAuthentication(username, password));

            var client = new ElasticsearchClient(settings);

            builder.Services.AddSingleton(client);

            builder.Services.AddScoped<ICoursesQueryService, CoursesQueryService>();
            builder.Services.AddScoped<ICoursesCommandService, CoursesCommandService>();
        }

        private static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddRabbitMQEventBus(c =>
            {
                c.UriString = builder.Configuration["RABBITMQ_URI"] ?? "";

                c.ConfigureConsume<CourseInfoUpdatedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.info.updated";
                    opt.QueueOptions.QueueName = "search_service_course.info.updated";
                });
                c.ConfigureConsume<CoursePublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.published";
                    opt.QueueOptions.QueueName = "search_service_course.published";
                });
                c.ConfigureConsume<CourseUnpublishedIntegrationEvent>(opt =>
                {
                    opt.ExchangeOptions.ExchangeName = "course.unpublished";
                    opt.QueueOptions.QueueName = "search_service_course.unpublished";
                });
            })
            .AddIntegrationEventHandler<CourseInfoUpdatedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CoursePublishedIntegrationEventHandler>()
            .AddIntegrationEventHandler<CourseUnpublishedIntegrationEventHandler>();
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
