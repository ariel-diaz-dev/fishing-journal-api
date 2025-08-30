using Domain.DTOs.Common;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class FishSpeciesService : IFishSpeciesService
{
    private readonly IFishSpeciesRepository _fishSpeciesRepository;

    public FishSpeciesService(IFishSpeciesRepository fishSpeciesRepository)
    {
        _fishSpeciesRepository = fishSpeciesRepository;
    }

    public async Task<PaginatedResponse<FishSpecies>> GetAllFishSpeciesAsync(CancellationToken cancellationToken = default)
    {
        return await _fishSpeciesRepository.GetAllFishSpeciesPaginatedAsync(25, null, cancellationToken);
    }

    public async Task<PaginatedResponse<FishSpecies>> GetAllFishSpeciesPaginatedAsync(int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        return await _fishSpeciesRepository.GetAllFishSpeciesPaginatedAsync(limit, cursor, cancellationToken);
    }
}