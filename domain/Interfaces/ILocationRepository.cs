using Domain.Models;

namespace Domain.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
}