using CourseService.Application.Exceptions;
using CourseService.Application.IntegrationEvents.User;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using EventBus;
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

            var user = await _userRepository.FindOneAsync(@event.Id) ?? throw new UserNotFoundException(@event.Id);
            
            bool nameOrThumbnailUpdated = false;
            if (@event.Name != user.Name || @event.ThumbnailUrl != user.ThumbnailUrl)
            {
                nameOrThumbnailUpdated = true;
            }

            user.UpdateInfoIgnoreNull(@event.Name, @event.ThumbnailUrl, @event.EnrolledCoursesVisible);
            
            await _userRepository.UpdateAsync(user);
            
            if (nameOrThumbnailUpdated)
            {
                var userInCourse = new UserInCourse(user.Id, user.Name, user.ThumbnailUrl);

                await _courseRepository.UpdateUserInCoursesAsync(userInCourse);
            }
        }
    }
}
