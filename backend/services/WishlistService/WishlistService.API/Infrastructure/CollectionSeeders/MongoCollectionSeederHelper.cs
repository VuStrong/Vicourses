using MongoDB.Driver;
using System.Reflection;

namespace WishlistService.API.Infrastructure.CollectionSeeders
{
    public static class MongoCollectionSeederHelper
    {
        public static async Task SeedMongoCollections(this IHost app)
        {
            var types = typeof(MongoCollectionSeeder<>).Assembly.GetTypes();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            foreach (var type in types)
            {
                var documentType = GetDocumentType(type);

                if (documentType == null) continue;

                var collectionType = typeof(IMongoCollection<>).MakeGenericType(documentType);
                var collection = services.GetService(collectionType);

                if (collection == null) continue;

                var seederInstance = Activator.CreateInstance(type);
                var seedMethod = type.GetMethod("SeedAsync", BindingFlags.Instance | BindingFlags.Public);

                var seedTask = seedMethod!.Invoke(seederInstance, new object[] { collection }) as Task;
                await seedTask!;
            }
        }

        private static Type? GetDocumentType(Type seederType)
        {
            while (!seederType.IsGenericType || seederType.GetGenericTypeDefinition() != typeof(MongoCollectionSeeder<>))
            {
                if (seederType.BaseType != null)
                {
                    seederType = seederType.BaseType;
                }
                else
                {
                    return null;
                }
            }

            return seederType.GetGenericArguments()[0];
        }
    }
}
