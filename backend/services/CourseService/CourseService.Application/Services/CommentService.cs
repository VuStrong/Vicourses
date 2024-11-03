using AutoMapper;
using CourseService.Application.Dtos.Comment;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using CourseService.Domain.Services;
using CourseService.Shared.Paging;

namespace CourseService.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentDomainService _commentDomainService;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            ILessonRepository lessonRepository,
            IUserRepository userRepository,
            ICommentDomainService commentDomainService,
            IDomainEventDispatcher domainEventDispatcher,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _lessonRepository = lessonRepository;
            _userRepository = userRepository;
            _commentDomainService = commentDomainService;
            _domainEventDispatcher = domainEventDispatcher;
            _mapper = mapper;
        }

        public async Task<PagedResult<CommentDto>> GetCommentsAsync(GetCommentsParamsDto paramsDto, CancellationToken cancellationToken = default)
        {
            int skip = paramsDto.Skip < 0 ? 0 : paramsDto.Skip;
            int limit = paramsDto.Limit <= 0 ? 10 : paramsDto.Limit;

            var results = await _commentRepository.FindByLessonIdAsync(
                paramsDto.LessonId, skip, limit,
                sort: paramsDto.Sort,
                replyToId: paramsDto.ReplyToId,
                cancellationToken: cancellationToken
            );

            return _mapper.Map<PagedResult<CommentDto>>(results);
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto data)
        {
            var lesson = await _lessonRepository.FindOneAsync(data.LessonId) ?? throw new LessonNotFoundException(data.LessonId);

            var user = await _userRepository.FindOneAsync(data.UserId) ?? throw new UserNotFoundException(data.UserId);

            Comment? replyTo = null;
            if (data.ReplyToId != null)
            {
                replyTo = await _commentRepository.FindOneAsync(data.ReplyToId) ?? throw new CommentNotFoundException(data.ReplyToId);
            }

            var comment = await _commentDomainService.CreateCommentAsync(lesson, user, data.Content, replyTo);

            await _commentRepository.CreateAsync(comment);

            _ = _domainEventDispatcher.DispatchFrom(comment);

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task UpvoteCommentAsync(string commentId, string userId)
        {
            var comment = await _commentRepository.FindOneAsync(commentId) ?? throw new CommentNotFoundException(commentId);

            comment.Upvote(userId);

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task CancelUpvoteAsync(string commentId, string userId)
        {
            var comment = await _commentRepository.FindOneAsync(commentId) ?? throw new CommentNotFoundException(commentId);

            comment.RemoveUpvote(userId);

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(string commentId, string userId)
        {
            var comment = await _commentRepository.FindOneAsync(commentId) ?? throw new CommentNotFoundException(commentId);

            if (comment.User.Id != userId)
            {
                var lesson = await _lessonRepository.FindOneAsync(comment.LessonId) ?? throw new LessonNotFoundException(comment.LessonId);

                if (lesson.UserId != userId)
                {
                    throw new ForbiddenException("Forbidden action!");
                }
            }

            comment.SetDeleted();

            await _commentRepository.UpdateAsync(comment);
        }
    }
}
