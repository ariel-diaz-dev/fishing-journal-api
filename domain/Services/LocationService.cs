using Domain.DTOs.Common;
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

    public async Task<PaginatedResponse<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _locationRepository.GetAllLocationsPaginatedAsync(25, null, cancellationToken);
    }

    public async Task<PaginatedResponse<Location>> GetAllLocationsPaginatedAsync(int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        return await _locationRepository.GetAllLocationsPaginatedAsync(limit, cursor, cancellationToken);
    }
}