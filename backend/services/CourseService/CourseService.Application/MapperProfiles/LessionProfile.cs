using AutoMapper;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Dtos.Quiz;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class LessionProfile : Profile
    {
        public LessionProfile()
        {
            CreateMap<Lession, LessionDto>();

            CreateMap<Answer, QuizAnswerDto>();
            CreateMap<Quiz, QuizDto>();
        }
    }
}
