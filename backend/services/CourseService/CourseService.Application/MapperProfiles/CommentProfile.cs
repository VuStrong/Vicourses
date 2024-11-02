using AutoMapper;
using CourseService.Application.Dtos.Comment;
using CourseService.Domain.Models;

namespace CourseService.Application.MapperProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<UserInComment, UserInCommentDto>();
            CreateMap<Comment, CommentDto>();
        }
    }
}
