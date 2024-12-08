﻿using RatingService.API.Application.Dtos.Rating;
using RatingService.API.Application.Dtos;

namespace RatingService.API.Application.Services
{
    public interface IRatingService
    {
        Task<PagedResult<RatingDto>> GetRatingsByCourseAsync(GetRatingsParamsDto paramsDto, CancellationToken cancellationToken = default);

        Task<RatingDto> CreateRatingAsync(CreateRatingDto data);
        Task<RatingDto> UpdateRatingAsync(string ratingId, UpdateRatingDto data);
        Task DeleteRatingAsync(string ratingId, string userId);

        Task<RatingDto> RespondRatingAsync(string ratingId, RespondRatingDto data);
    }
}
