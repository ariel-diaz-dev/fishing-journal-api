using Domain.Models;

namespace Domain.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
}