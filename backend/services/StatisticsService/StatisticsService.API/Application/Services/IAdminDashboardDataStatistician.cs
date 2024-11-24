﻿using StatisticsService.API.Application.Dtos;

namespace StatisticsService.API.Application.Services
{
    public interface IAdminDashboardDataStatistician
    {
        Task<AdminDashboardDataDto> GetAdminDashboardDataAsync(CancellationToken cancellationToken = default);
    }
}