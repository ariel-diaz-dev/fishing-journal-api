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

    public async Task<IEnumerable<FishSpecies>> GetAllFishSpeciesAsync(CancellationToken cancellationToken = default)
    {
        return await _fishSpeciesRepository.GetAllFishSpeciesAsync(cancellationToken);
    }
}