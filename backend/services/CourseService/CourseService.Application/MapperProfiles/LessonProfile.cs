using AutoMapper;
using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Dtos.Quiz;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, LessonDto>();

            CreateMap<Answer, QuizAnswerDto>();
            CreateMap<Quiz, QuizDto>();
        }
    }
}
