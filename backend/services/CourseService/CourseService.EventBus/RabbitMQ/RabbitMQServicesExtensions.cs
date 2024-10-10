using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CourseService.EventBus.RabbitMQ
{
    public static class RabbitMQServicesExtensions
    {
        public static EventBusBuilder AddRabbitMQEventBus(
            this IServiceCollection services,
            Action<RabbitMQEventBusConfigurator>? config = null)
        {
            var builder = new EventBusBuilder(services);

            var configurator = new RabbitMQEventBusConfigurator();

            config?.Invoke(configurator);

            services.AddSingleton<RabbitMQEventBusConfigurator>(s => configurator);

            services.AddSingleton<IConnection>(s =>
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(configurator.UriString),
                    DispatchConsumersAsync = true,
                };

                return factory.CreateConnection();
            });

            services.AddSingleton<IEventBus, RabbitMQEventBus>();

            services.AddHostedService<RabbitMQEventBus>(s => (RabbitMQEventBus)s.GetRequiredService<IEventBus>());

            return builder;
        }
    }
}
