using Domain.Models;

namespace Domain.Interfaces;

public interface ILandingRepository
{
    Task<Landing?> GetByIdAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Landing>> GetByFishingReportIdAsync(Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Landing> Items, string? NextCursor)> GetByFishingReportIdPaginatedAsync(Guid fishingReportId, Guid accountId, int limit, string? cursor = null, CancellationToken cancellationToken = default);
    Task<Landing> AddAsync(Landing landing, CancellationToken cancellationToken = default);
    Task<Landing> UpdateAsync(Landing landing, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default);
}