using Domain.DTOs.Common;
using Domain.DTOs.FishingReport;

namespace Domain.Interfaces;

public interface IFishingReportService
{
    Task<FishingReportDto?> GetFishingReportByIdAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FishingReportDto>> GetFishingReportsByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FishingReportDto>> GetFishingReportsByAccountIdPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default);
    Task<FishingReportDto> CreateFishingReportAsync(CreateFishingReportDto createFishingReportDto, Guid accountId, CancellationToken cancellationToken = default);
    Task<FishingReportDto?> UpdateFishingReportAsync(Guid id, UpdateFishingReportDto updateFishingReportDto, Guid accountId, CancellationToken cancellationToken = default);
    Task<bool> DeleteFishingReportAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default);
}