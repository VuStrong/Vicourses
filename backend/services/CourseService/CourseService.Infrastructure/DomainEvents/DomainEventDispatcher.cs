using CourseService.Domain;
using CourseService.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

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
            using var scope = _serviceScopeFactory.CreateScope();

            var handlers = scope.ServiceProvider.GetKeyedServices<IDomainEventHandler>(domainEvent.GetType());

            if (!handlers.Any()) return;

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.Handle(domainEvent);
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
