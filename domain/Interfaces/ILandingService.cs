using Domain.DTOs.Common;
using Domain.DTOs.Landing;

namespace Domain.Interfaces;

public interface ILandingService
{
    Task<LandingDto?> GetLandingByIdAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<LandingDto>> GetLandingsByFishingReportIdAsync(Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<LandingDto>> GetLandingsByFishingReportIdPaginatedAsync(Guid fishingReportId, Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default);
    Task<LandingDto> CreateLandingAsync(CreateLandingDto createLandingDto, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
    Task<LandingDto?> UpdateLandingAsync(Guid id, UpdateLandingDto updateLandingDto, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
    Task<bool> DeleteLandingAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
}