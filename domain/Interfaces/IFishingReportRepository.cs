using Domain.DTOs.Common;
using Domain.Models;

namespace Domain.Interfaces;

public interface IFishingReportRepository
{
    Task<FishingReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FishingReport>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FishingReport>> GetByAccountIdPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default);
    Task<FishingReport> AddAsync(FishingReport fishingReport, CancellationToken cancellationToken = default);
    Task UpdateAsync(FishingReport fishingReport, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> BelongsToAccountAsync(Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
}