using CourseService.Application.IntegrationEvents.User;
using CourseService.Domain.Contracts;
using EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.User
{
    public class UserInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserInfoUpdatedIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserInfoUpdatedIntegrationEventHandler> _logger;

        public UserInfoUpdatedIntegrationEventHandler(
            IUserRepository userRepository,
            ILogger<UserInfoUpdatedIntegrationEventHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(UserInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"CourseService handle {@event.GetType().Name}: {@event.Id}");

            var user = await _userRepository.FindOneAsync(@event.Id);
            
            if (user != null)
            {
                await UpdateUser(user, @event);
            }
            else
            {
                await AddUser(@event);
            }
        }

        private async Task AddUser(UserInfoUpdatedIntegrationEvent @event)
        {
            var user = Domain.Models.User.Create(@event.Id, @event.Name, @event.Email, @event.ThumbnailUrl);

            await _userRepository.CreateAsync(user);
        }

        private async Task UpdateUser(Domain.Models.User user, UserInfoUpdatedIntegrationEvent @event)
        {
            user.UpdateInfoIgnoreNull(@event.Name, @event.ThumbnailUrl, @event.EnrolledCoursesVisible);
            
            await _userRepository.UpdateAsync(user);
        }
    }
}
