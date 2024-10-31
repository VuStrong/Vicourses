using SearchService.API.Application.Dtos;
using SearchService.API.Models;

namespace SearchService.API.Application.Interfaces
{
    public interface ICoursesQueryService
    {
        Task<PagedResult<Course>> SearchCoursesAsync(SearchCoursesParams searchParams, CancellationToken cancellationToken = default);
    }
}
