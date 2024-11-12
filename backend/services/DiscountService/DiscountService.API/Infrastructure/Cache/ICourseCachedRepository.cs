namespace DiscountService.API.Infrastructure.Cache
{
    public interface ICourseCachedRepository
    {
        /// <summary>
        /// Get course's price by courseId
        /// </summary>
        /// <returns>A decimal if the course exists, otherwise null</returns>
        Task<decimal?> GetCoursePriceAsync(string courseId);

        Task DeleteCoursePriceCachedAsync(string courseId);
    }
}
