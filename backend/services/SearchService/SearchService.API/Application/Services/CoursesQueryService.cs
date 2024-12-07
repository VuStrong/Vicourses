using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using SearchService.API.Application.Dtos;
using SearchService.API.Application.Exceptions;
using SearchService.API.Application.Interfaces;
using SearchService.API.Models;

namespace SearchService.API.Application.Services
{
    public class CoursesQueryService : ICoursesQueryService
    {
        private readonly ElasticsearchClient _client;
        private const string CourseIndex = "courses";

        public CoursesQueryService(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<PagedResult<Course>> SearchCoursesAsync(SearchCoursesParams searchParams, CancellationToken cancellationToken = default)
        {
            if (searchParams.Skip < 0) searchParams.Skip = 0;
            searchParams.Limit = Math.Clamp(searchParams.Limit, 1, 100);
            searchParams.Keyword = searchParams.Keyword?.Trim();

            var descriptor = new SearchRequestDescriptor<Course>()
                .Index(CourseIndex)
                .From(searchParams.Skip)
                .Size(searchParams.Limit);

            BuildQuery(descriptor, searchParams);

            Sort(descriptor, searchParams.Sort);

            var response = await _client.SearchAsync(descriptor, cancellationToken);

            if (!response.IsValidResponse)
            {
                throw new InternalServerException("Cannot search for courses");
            }

            var courses = response.Documents.ToList();
            var total = response.Total;

            return new PagedResult<Course>(courses, searchParams.Skip, searchParams.Limit, total);
        }

        private static void BuildQuery(SearchRequestDescriptor<Course> descriptor, SearchCoursesParams searchParams)
        {
            if (string.IsNullOrEmpty(searchParams.Keyword) && 
                searchParams.Rating == null && 
                searchParams.Level == null &&
                searchParams.Free == null)
            {
                return;
            }

            List<Action<QueryDescriptor<Course>>> mustQueries = [];

            if (!string.IsNullOrEmpty(searchParams.Keyword))
            {
                mustQueries.Add(must => must
                    .Bool(b2 => b2
                        .Should(should => should
                            .MultiMatch(mm => mm
                                .Query(searchParams.Keyword)
                                .Fields(new Field("title").And("tags"))
                            )
                        )
                    )
                );
            }

            if (searchParams.Rating != null)
            {
                mustQueries.Add(must => must
                    .Range(range => range
                        .NumberRange(nr => nr
                            .Field(new Field("rating"))
                            .Gte(decimal.ToDouble(searchParams.Rating.Value))
                        )
                    )
                );
            }

            if (searchParams.Level != null)
            {
                mustQueries.Add(must => must
                    .Match(match => match
                        .Query(searchParams.Level.Value.ToString())
                        .Field(new Field("level"))
                    )
                );
            }

            if (searchParams.Free != null)
            {
                mustQueries.Add(must => must
                    .Term(term => term
                        .Field(new Field("isPaid"))
                        .Value(!searchParams.Free.Value)
                    )
                );
            }

            descriptor = descriptor.Query(q =>
            {
                q.Bool(b =>
                {
                    b.Must(mustQueries.ToArray());
                });
            });
        }

        private static void Sort(SearchRequestDescriptor<Course> descriptor, CourseSort sort)
        {
            switch (sort)
            {
                case CourseSort.Newest:
                    descriptor = descriptor.Sort(s => 
                        s.Field(f => f.CreatedAt, new FieldSort { Order = SortOrder.Desc }));
                    break;
                case CourseSort.HighestRated:
                    descriptor = descriptor.Sort(s =>
                        s.Field(f => f.Rating, new FieldSort { Order = SortOrder.Desc }));
                    break;
            }
        }
    }
}
