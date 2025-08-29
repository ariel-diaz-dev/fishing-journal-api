using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _locationRepository.GetAllLocationsAsync(cancellationToken);
    }
}