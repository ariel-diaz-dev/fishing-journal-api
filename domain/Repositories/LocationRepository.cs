using Domain.Data;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .OrderBy(l => l.Order)
            .ToListAsync(cancellationToken);
    }
}