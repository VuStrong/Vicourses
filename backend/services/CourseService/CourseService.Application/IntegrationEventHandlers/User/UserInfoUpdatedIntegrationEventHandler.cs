using CourseService.Application.Exceptions;
using CourseService.Application.IntegrationEvents.User;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using CourseService.EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.User
{
    public class UserInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserInfoUpdatedIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<UserInfoUpdatedIntegrationEventHandler> _logger;

        public UserInfoUpdatedIntegrationEventHandler(
            IUserRepository userRepository,
            ICourseRepository courseRepository,
            ILogger<UserInfoUpdatedIntegrationEventHandler> logger)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task Handle(UserInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"CourseService handle {@event.GetType().Name}: {@event.Id}");

            var user = await _userRepository.FindOneAsync(@event.Id);

            if (user == null) throw new UserNotFoundException(@event.Id);

            if (@event.Name != user.Name || @event.ThumbnailUrl != user.ThumbnailUrl)
            {
                user.UpdateInfoIgnoreNull(@event.Name, @event.ThumbnailUrl);

                var userInCourse = new UserInCourse(user.Id, user.Name, user.ThumbnailUrl);

                await _userRepository.UpdateAsync(user);

                await _courseRepository.UpdateUserInCoursesAsync(userInCourse);
            }
        }
    }
}
