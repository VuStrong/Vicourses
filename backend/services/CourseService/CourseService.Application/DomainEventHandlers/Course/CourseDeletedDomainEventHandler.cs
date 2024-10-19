using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseDeletedDomainEventHandler : IDomainEventHandler<CourseDeletedDomainEvent>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;

        public CourseDeletedDomainEventHandler(
            ISectionRepository sectionRepository,
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository)
        {
            _sectionRepository = sectionRepository;
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
        }

        public async Task Handle(CourseDeletedDomainEvent @event)
        {
            var lessons = await _lessonRepository.FindByCourseIdAsync(@event.Course.Id);
            var videoFiles = lessons.Where(l => l.Video != null).Select(l => l.Video);
            var quizLessonIds = lessons.Where(l => l.Type == Domain.Enums.LessonType.Quiz).Select(l => l.Id);

            await _sectionRepository.DeleteByCourseIdAsync(@event.Course.Id);

            await _lessonRepository.DeleteByCourseIdAsync(@event.Course.Id);

            if (quizLessonIds.Any()) 
                await _quizRepository.DeleteByLessonIdsAsync(quizLessonIds);

            // todo: Send files to storage service to remove
        }
    }
}
