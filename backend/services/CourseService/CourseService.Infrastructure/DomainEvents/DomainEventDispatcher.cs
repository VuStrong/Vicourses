using CourseService.Domain;
using CourseService.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CourseService.Infrastructure.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DomainEventDispatcher(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Dispatch(DomainEvent domainEvent)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            using var scope = _serviceScopeFactory.CreateScope();

            var handlers = scope.ServiceProvider.GetServices(handlerType);

            if (!handlers.Any()) return;

            foreach ( var handler in handlers)
            {
                var method = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);

                try
                {
                    var task = method!.Invoke(handler, new object[] { domainEvent }) as Task;

                    await task!;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while handling domain event {domainEvent.GetType().Name}: {ex.Message}");
                }
            }
        }

        public async Task DispatchFrom(Entity entity)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                await Dispatch(domainEvent);
            }

            entity.ClearDomainEvents();
        }
    }
}
