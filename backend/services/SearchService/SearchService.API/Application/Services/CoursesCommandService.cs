using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using SearchService.API.Application.Configurations;
using SearchService.API.Application.Exceptions;
using SearchService.API.Application.Interfaces;
using SearchService.API.Models;

namespace SearchService.API.Application.Services
{
    public class CoursesCommandService : ICoursesCommandService
    {
        private readonly ElasticsearchClient _client;
        private readonly string CourseIndex;

        public CoursesCommandService(ElasticsearchClient client, IOptions<ElasticsearchConfiguration> configuration)
        {
            _client = client;
            CourseIndex = configuration.Value.CourseIndexName;
        }

        public async Task InsertOrUpdateCourseAsync(Course course, CancellationToken cancellationToken = default)
        {
            var response = await _client.IndexAsync(course, index: CourseIndex, cancellationToken);

            if (!response.IsValidResponse)
            {
                throw new InternalServerException($"Cannot create or update course {course.Id}");
            }
        }

        public async Task DeleteCourseAsync(string courseId, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteAsync(index: CourseIndex, courseId, cancellationToken);

            if (!response.IsValidResponse)
            {
                if (response.Result == Result.NotFound)
                {
                    throw new NotFoundException(CourseIndex, courseId);
                }

                throw new InternalServerException($"Cannot delete course {courseId}");
            }
        }
    }
}