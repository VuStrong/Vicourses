using Microsoft.Extensions.DependencyInjection;

namespace CourseService.EventBus
{
    public class EventBusBuilder
    {
        private readonly IServiceCollection _services;

        public EventBusBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public EventBusBuilder AddIntegrationEventHandler<T>() where T : IIntegrationEventHandler
        {
            var handlerType = typeof(T);
            var eventType = GetEventType(handlerType);

            if (eventType == null) return this;

            _services.AddKeyedScoped(typeof(IIntegrationEventHandler), eventType.Name, handlerType);

            return this;
        }

        private static Type? GetEventType(Type handlerType)
        {
            if (handlerType.IsInterface) return null;

            var type = handlerType.GetInterfaces().FirstOrDefault(x => 
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));

            return type?.GetGenericArguments()[0];
        }
    }
}
