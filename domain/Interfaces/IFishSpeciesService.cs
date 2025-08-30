using Domain.DTOs.Common;
using Domain.Models;

namespace Domain.Interfaces;

public interface IFishSpeciesService
{
    Task<PaginatedResponse<FishSpecies>> GetAllFishSpeciesAsync(CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FishSpecies>> GetAllFishSpeciesPaginatedAsync(int limit = 25, string? cursor = null, CancellationToken cancellationToken = default);
}