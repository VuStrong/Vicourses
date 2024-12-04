using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(IMongoCollection<Comment> collection, IDomainEventDispatcher dispatcher) : 
            base(collection, dispatcher) { }

        public async Task<PagedResult<Comment>> FindByLessonIdAsync(string lessonId, int skip, int limit, CommentSort? sort = null,
            string? replyToId = null, CancellationToken cancellationToken = default)
        {
            var builder = Builders<Comment>.Filter;
            var filter = builder.Eq(c => c.LessonId, lessonId) & builder.Eq(c => c.ReplyToId, replyToId);

            var fluent = _collection.Find(filter);

            if (sort != null)
            {
                fluent = Sort(fluent, sort.Value);
            }

            var comments = await fluent.Skip(skip).Limit(limit).ToListAsync(cancellationToken);

            var total = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            return new PagedResult<Comment>(comments, skip, limit, total);
        }

        private IFindFluent<Comment, Comment> Sort(IFindFluent<Comment, Comment> fluent, CommentSort sort)
        {
            switch (sort)
            {
                case CommentSort.Newest:
                    return fluent.SortByDescending(c => c.CreatedAt);
                case CommentSort.Oldest:
                    return fluent.SortBy(c => c.CreatedAt);
                case CommentSort.HighestUpvoted:
                    return fluent.SortByDescending(c => c.UpvoteCount);
                default:
                    throw new ArgumentException("Invalid sort value", nameof(sort));
            }
        }
    }
}
