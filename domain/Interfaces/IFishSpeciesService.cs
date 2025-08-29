using Domain.Models;

namespace Domain.Interfaces;

public interface IFishSpeciesService
{
    Task<IEnumerable<FishSpecies>> GetAllFishSpeciesAsync(CancellationToken cancellationToken = default);
}