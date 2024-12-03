using AutoMapper;
using EventBus;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RatingService.API.Application.Dtos;
using RatingService.API.Application.Dtos.Rating;
using RatingService.API.Application.Exceptions;
using RatingService.API.Application.IntegrationEvents.Rating;
using RatingService.API.Infrastructure;
using RatingService.API.Models;

namespace RatingService.API.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly RatingServiceDbContext _dbContext;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public RatingService(RatingServiceDbContext context, IEventBus eventBus, IMapper mapper)
        {
            _dbContext = context;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<PagedResult<RatingDto>> GetRatingsByCourseAsync(GetRatingsParamsDto paramsDto, CancellationToken cancellationToken = default)
        {
            int skip = paramsDto.Skip < 0 ? 0 : paramsDto.Skip;
            int limit = paramsDto.Limit <= 0 ? 15 : paramsDto.Limit;

            var query = _dbContext.Ratings.Where(r => r.CourseId == paramsDto.CourseId);

            if (paramsDto.Star != null)
            {
                query = query.Where(r => r.Star == paramsDto.Star.Value);
            }
            if (paramsDto.Responded != null)
            {
                query = query.Where(r => r.Responded == paramsDto.Responded.Value);
            }

            var total = await query.CountAsync(cancellationToken);

            query = query.Include(r => r.User).OrderByDescending(r => r.CreatedAt).Skip(skip).Take(limit);
            var ratings = await query.ToListAsync(cancellationToken);

            return new PagedResult<RatingDto>(
                _mapper.Map<List<RatingDto>>(ratings), skip, limit, total
            );
        }

        public async Task<RatingDto> CreateRatingAsync(CreateRatingDto data)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == data.CourseId);
            if (course == null)
            {
                throw new NotFoundException("course", data.CourseId);
            }

            if (!await CheckUserEnrolled(course.Id, data.UserId))
            {
                throw new ForbiddenException("Forbidden resource");
            }
            if (course.Status != CourseStatus.Published)
            {
                throw new ForbiddenException("Forbidden resource");
            }
            if (course.InstructorId == data.UserId)
            {
                throw new ForbiddenException("Cannot rating owned course");
            }

            var rating = new Rating(data.CourseId, data.UserId, data.Feedback, data.Star);
            _dbContext.Ratings.Add(rating);

            var newAvgRating = (course.AvgRating * course.RatingCount + rating.Star) / (course.RatingCount + 1);

            course.AvgRating = newAvgRating;
            course.RatingCount++;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as MySqlException;

                if (sqlException?.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    throw new ForbiddenException("An user can only rating a course once");
                }
                else if (sqlException?.ErrorCode == MySqlErrorCode.NoReferencedRow2)
                {
                    throw new NotFoundException("user", data.UserId);
                }

                throw;
            }

            PublishCourseRatingUpdatedIntegrationEvent(course);

            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<RatingDto> UpdateRatingAsync(string ratingId, UpdateRatingDto data)
        {
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == ratingId);
            if (rating == null)
            {
                throw new NotFoundException("rating", ratingId);
            }

            if (rating.UserId != data.UserId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == rating.CourseId);
            if (course == null)
            {
                throw new NotFoundException("course", rating.CourseId);
            }

            var updated = false;
            var ratingUpdated = false;
            if (data.Star != null && data.Star.Value != rating.Star)
            {
                var difference = data.Star.Value - rating.Star;

                rating.Star = data.Star.Value;

                var newAvgRating = (course.AvgRating * course.RatingCount + difference) / course.RatingCount;

                course.AvgRating = newAvgRating;

                updated = true;
                ratingUpdated = true;
            }
            if (!string.IsNullOrEmpty(data.Feedback))
            {
                rating.Feedback = data.Feedback;

                updated = true;
            }

            if (updated) await _dbContext.SaveChangesAsync();

            if (ratingUpdated) PublishCourseRatingUpdatedIntegrationEvent(course);

            return _mapper.Map<RatingDto>(rating);
        }

        public async Task DeleteRatingAsync(string ratingId, string userId)
        {
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == ratingId);
            if (rating == null)
            {
                throw new NotFoundException("rating", ratingId);
            }

            if (rating.UserId != userId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == rating.CourseId);
            if (course == null)
            {
                throw new NotFoundException("course", rating.CourseId);
            }

            var newAvgRating = (course.AvgRating * course.RatingCount - rating.Star) / (course.RatingCount - 1);
            course.AvgRating = newAvgRating;
            course.RatingCount--;

            _dbContext.Ratings.Remove(rating);

            await _dbContext.SaveChangesAsync();

            PublishCourseRatingUpdatedIntegrationEvent(course);
        }

        public async Task<RatingDto> RespondRatingAsync(string ratingId, RespondRatingDto data)
        {
            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == ratingId);
            if (rating == null)
            {
                throw new NotFoundException("rating", ratingId);
            }

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == rating.CourseId);
            if (course == null)
            {
                throw new NotFoundException("course", rating.CourseId);
            }

            if (data.UserId != course.InstructorId)
            {
                throw new ForbiddenException("Only instructor of the course can respond its rating");
            }

            rating.Response = data.Response;
            rating.Responded = true;
            rating.RespondedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RatingDto>(rating);
        }

        private void PublishCourseRatingUpdatedIntegrationEvent(Course course)
        {
            var @event = new CourseRatingUpdatedIntegrationEvent
            {
                Id = course.Id,
                AvgRating = course.AvgRating
            };

            _eventBus.Publish(@event);
        }

        private async Task<bool> CheckUserEnrolled(string courseId, string userId)
        {
            return await _dbContext.Enrollments.AnyAsync(e => e.CourseId == courseId && e.UserId == userId);
        }
    }
}
