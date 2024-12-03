using AutoMapper;
using CourseService.Application.Dtos.Lesson;
using CourseService.Domain.Models;
using CourseService.Domain.Objects;

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
