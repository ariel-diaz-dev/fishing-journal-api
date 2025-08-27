using Domain.DTOs.Tackle;
using Domain.Models;

namespace Domain.Interfaces;

public interface ITackleService
{
    Task<Tackle?> GetTackleByIdAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tackle>> GetAllTackleByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<Tackle> CreateTackleAsync(Guid accountId, CreateTackleDto createTackleDto, CancellationToken cancellationToken = default);
    Task<Tackle?> UpdateTackleAsync(Guid id, Guid accountId, UpdateTackleDto updateTackleDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTackleAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default);
}