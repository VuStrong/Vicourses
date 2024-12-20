using System.ComponentModel.DataAnnotations;
using PaymentService.API.Models;

namespace PaymentService.API.Requests
{
    public class GetPaymentsRequest
    {
        /// <summary>
        /// Skip the specified number of items
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Limit the number of items returned
        /// </summary>
        [Range(Int32.MinValue, 100, ErrorMessage = "Limit cannot greater than 100")]
        public int Limit { get; set; } = 15;

        /// <summary>
        /// Filter by User ID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Filter by payment status
        /// </summary>
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus? Status { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}