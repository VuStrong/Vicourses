using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountService.FunctionalTests
{
    internal static class Extensions
    {
        public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
        {
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<T>();

            context.Database.EnsureCreated();
        }
    }
}
