using Domain.Models;

namespace Domain.Interfaces;

public interface IFishSpeciesRepository
{
    Task<IEnumerable<FishSpecies>> GetAllFishSpeciesAsync(CancellationToken cancellationToken = default);
}