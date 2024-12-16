using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Category;
using CourseService.Domain.Events.Comment;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Events.Enrollment;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Events.Section;
using CourseService.Domain.Events.User;
using CourseService.Domain.Models;
using CourseService.Infrastructure.ClassMaps;
using CourseService.Infrastructure.CollectionSeeders;
using CourseService.Infrastructure.DomainEvents;
using CourseService.Infrastructure.DomainEvents.Handlers;
using CourseService.Infrastructure.Repositories;
using CourseService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace CourseService.Infrastructure
{
    public static class Extensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, Action<InfrastructureConfiguration>? config = null)
        {
            var configuration = new InfrastructureConfiguration();
            config?.Invoke(configuration);

            services.AddDbContext(configuration.DbConnectionString, configuration.DbName);

            CourseMap.Configure();
            SectionMap.Configure();
            LessonMap.Configure();
            UserMap.Configure();
            CategoryMap.Configure();
            EnrollmentMap.Configure();
            CommentMap.Configure();
            VideoFileMap.Congifure();
            AppDtoMap.Configure();

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<ICourseCurriculumManager, CourseCurriculumManager>();
            services.AddScoped<IDataAggregator, DataAggregator>();

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            services.AddDomainEventHandlers();
        }

        private static void AddDomainEventHandler<T, TH>(this IServiceCollection services)
            where T : DomainEvent
            where TH : class, IDomainEventHandler<T>
        {
            services.AddKeyedScoped<IDomainEventHandler, TH>(typeof(T));
        }

        private static void AddDomainEventHandlers(this IServiceCollection services)
        {
            services.AddDomainEventHandler<CategoryDeletedDomainEvent, CategoryDeletedDomainEventHandler>();
            services.AddDomainEventHandler<CommentCreatedDomainEvent, CommentCreatedDomainEventHandler>();
            services.AddDomainEventHandler<CourseDeletedDomainEvent, CourseDeletedDomainEventHandler>();
            services.AddDomainEventHandler<LessonCreatedDomainEvent, LessonCreatedDomainEventHandler>();
            services.AddDomainEventHandler<LessonDeletedDomainEvent, LessonDeletedDomainEventHandler>();
            services.AddDomainEventHandler<LessonVideoProcessedDomainEvent, LessonVideoProcessedDomainEventHandler>();
            services.AddDomainEventHandler<LessonVideoUpdatedDomainEvent, LessonVideoUpdatedDomainEventHandler>();
            services.AddDomainEventHandler<SectionCreatedDomainEvent, SectionCreatedDomainEventHandler>();
            services.AddDomainEventHandler<SectionDeletedDomainEvent, SectionDeletedDomainEventHandler>();
            services.AddDomainEventHandler<UserEnrolledDomainEvent, UserEnrolledDomainEventHandler>();
            services.AddDomainEventHandler<UserUnenrolledDomainEvent, UserUnenrolledDomainEventHandler>();
            services.AddDomainEventHandler<UserInfoUpdatedDomainEvent, UserInfoUpdatedDomainEventHandler>();
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString, string databaseName)
        {
            //var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            //mongoClientSettings.ClusterConfigurator = cb =>
            //{
            //    cb.Subscribe<CommandStartedEvent>(e =>
            //    {
            //        Console.WriteLine($"{e.CommandName} - {e.Command.ToString()}");
            //    });
            //};
            //var mongoCfgClient = new MongoClient(mongoClientSettings);
            //services.AddSingleton<IMongoClient>(mongoCfgClient);

            services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
            services.AddSingleton<IMongoDatabase>(s =>
            {
                var client = s.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            services.AddCollection<Course>(databaseName, "courses")
                .AddCollection<Section>(databaseName, "sections")
                .AddCollection<Lesson>(databaseName, "lessons")
                .AddCollection<Category>(databaseName, "categories")
                .AddCollection<User>(databaseName, "users")
                .AddCollection<Enrollment>(databaseName, "enrollments")
                .AddCollection<Comment>(databaseName, "comments");

            return services;
        }

        private static IServiceCollection AddCollection<T>(this IServiceCollection services, string databaseName, string collectionName)
        {
            services.AddScoped<IMongoCollection<T>>(s =>
            {
                var client = s.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(databaseName);
                return database.GetCollection<T>(collectionName);
            });

            return services;
        }

        public static async Task SeedDatabase(this IHost app)
        {
            await app.SeedMongoCollections();
        }
    }

    public class InfrastructureConfiguration
    {
        public string DbConnectionString { get; set; } = string.Empty;
        public string DbName { get; set; } = string.Empty;
    }
}
