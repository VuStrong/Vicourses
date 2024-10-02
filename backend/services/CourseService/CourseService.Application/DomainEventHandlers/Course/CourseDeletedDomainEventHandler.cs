using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseDeletedDomainEventHandler : IDomainEventHandler<CourseDeletedDomainEvent>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly ILessionRepository _lessionRepository;
        private readonly IQuizRepository _quizRepository;

        public CourseDeletedDomainEventHandler(
            ISectionRepository sectionRepository,
            ILessionRepository lessionRepository,
            IQuizRepository quizRepository)
        {
            _sectionRepository = sectionRepository;
            _lessionRepository = lessionRepository;
            _quizRepository = quizRepository;
        }

        public async Task Handle(CourseDeletedDomainEvent @event)
        {
            var lessions = await _lessionRepository.FindByCourseIdAsync(@event.Course.Id);
            var videoFiles = lessions.Where(l => l.Video != null).Select(l => l.Video);
            var quizLessionIds = lessions.Where(l => l.Type == Domain.Enums.LessionType.Quiz).Select(l => l.Id);

            await _sectionRepository.DeleteByCourseIdAsync(@event.Course.Id);

            await _lessionRepository.DeleteByCourseIdAsync(@event.Course.Id);

            if (quizLessionIds.Any()) 
                await _quizRepository.DeleteByLessionIdsAsync(quizLessionIds);

            // todo: Send files to storage service to remove
        }
    }
}
