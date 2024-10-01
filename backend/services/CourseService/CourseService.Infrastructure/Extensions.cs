using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using CourseService.Infrastructure.ClassMaps;
using CourseService.Infrastructure.CollectionSeeders;
using CourseService.Infrastructure.DomainEvents;
using CourseService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace CourseService.Infrastructure
{
    public static class Extensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            CourseMap.Configure();
            SectionMap.Configure();
            LessionMap.Configure();
            UserMap.Configure();
            CategoryMap.Configure();
            EnrollmentMap.Configure();
            QuizMap.Configure();

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ICourseCurriculumRepository, CourseCurriculumRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();

            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>(s =>
            {
                var scopeFactory = s.GetRequiredService<IServiceScopeFactory>();

                return new DomainEventDispatcher(scopeFactory);
            });
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString, string databaseName)
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    Console.WriteLine($"{e.CommandName} - {e.Command.ToString()}");
                });
            };
            var mongoCfgClient = new MongoClient(mongoClientSettings);
            services.AddSingleton<IMongoClient>(mongoCfgClient);

            //services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

            services.AddCollection<Course>(databaseName, "courses")
                .AddCollection<Section>(databaseName, "sections")
                .AddCollection<Lession>(databaseName, "lessions")
                .AddCollection<Category>(databaseName, "categories")
                .AddCollection<User>(databaseName, "users")
                .AddCollection<Enrollment>(databaseName, "enrollments")
                .AddCollection<Quiz>(databaseName, "quizzes");

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
}
