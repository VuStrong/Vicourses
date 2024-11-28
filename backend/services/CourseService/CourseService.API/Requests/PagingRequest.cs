using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests
{
    public class PagingRequest
    {
        /// <summary>
        /// Skip the specified number of items
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Limit the number of items returned
        /// </summary>
        [Range(Int32.MinValue, 100, ErrorMessage = "Limit cannot greater than 100")]
        public int Limit { get; set; }
    }
}
