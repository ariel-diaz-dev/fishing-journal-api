using Domain.DTOs.Common;
using Domain.Models;

namespace Domain.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
    Task<PaginatedResponse<Location>> GetAllLocationsPaginatedAsync(int limit = 25, string? cursor = null, CancellationToken cancellationToken = default);
}