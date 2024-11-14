using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Testcontainers.MySql;
using Testcontainers.Redis;
using Org.BouncyCastle.OpenSsl;
using Microsoft.AspNetCore.TestHost;
using DiscountService.API.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DiscountService.FunctionalTests
{
    public class DiscountApiTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MySqlContainer _dbContainer = new MySqlBuilder()
            .WithImage("mysql:8.4.0")
            .WithDatabase("discount_db")
            .WithUsername("vicourses")
            .WithPassword("password")
            .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.EnsureDbCreated<DiscountServiceDbContext>();
            });
        }

        public async Task ResetDatabaseAsync()
        {
            var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountServiceDbContext>();

            await dbContext.Courses.ExecuteDeleteAsync();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _redisContainer.StartAsync();

            Environment.SetEnvironmentVariable("ConnectionStrings__DiscountDB", _dbContainer.GetConnectionString());
            Environment.SetEnvironmentVariable("ConnectionStrings__Redis", _redisContainer.GetConnectionString());
            Environment.SetEnvironmentVariable("ConnectionStrings__RabbitMQ", "amqp://localhost:5672");

            PrepareRsaKeyPairForJwtAuthentication();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _redisContainer.StopAsync();

            Environment.SetEnvironmentVariable("ConnectionStrings__DiscountDB", "");
            Environment.SetEnvironmentVariable("ConnectionStrings__Redis", "");
            Environment.SetEnvironmentVariable("ConnectionStrings__RabbitMQ", "");

            if (File.Exists("private.key"))
            {
                File.Delete("private.key");
            }
            if (File.Exists("public.key"))
            {
                File.Delete("public.key");
            }
        }

        /// <summary>
        /// Create 2 files (private.key and public.key) to store rsa key pair for jwt authentication
        /// </summary>
        private static void PrepareRsaKeyPairForJwtAuthentication()
        {
            RsaKeyPairGenerator generator = new();

            generator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));

            AsymmetricCipherKeyPair pair = generator.GenerateKeyPair();

            using (var writer = new StreamWriter("private.key"))
            {
                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(pair.Private);
            }

            using (var writer = new StreamWriter("public.key"))
            {
                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(pair.Public);
            }
        }
    }
}
