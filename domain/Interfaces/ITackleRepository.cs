using Domain.DTOs.Common;
using Domain.Models;

namespace Domain.Interfaces;

public interface ITackleRepository
{
    Task<Tackle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tackle>> GetAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<Tackle>> GetAllByAccountIdPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default);
    Task<Tackle> AddAsync(Tackle tackle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tackle tackle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> BelongsToAccountAsync(Guid tackleId, Guid accountId, CancellationToken cancellationToken = default);
}