using CourseService.Application.Dtos.Category;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Requests.Category
{
    public class GetCategoriesRequest
    {
        /// <summary>
        /// Keyword to search categories
        /// </summary>
        [FromQuery(Name = "q")]
        public string? Keyword {  get; set; }

        /// <summary>
        /// Filter by parent category id (pass 'null' to get root categories)
        /// </summary>
        public string? ParentId { get; set; }

        public GetCategoriesParamsDto ToGetCategoriesDto()
        {
            return new GetCategoriesParamsDto
            {
                Keyword = Keyword,
                ParentId = ParentId
            };
        }
    }
}
