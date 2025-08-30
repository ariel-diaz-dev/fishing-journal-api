using Domain.Data;
using Domain.DTOs.Common;
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

    public async Task<PaginatedResponse<Location>> GetAllLocationsPaginatedAsync(int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Locations
            .OrderBy(l => l.Order)
            .ThenBy(l => l.Id);

        if (!string.IsNullOrEmpty(cursor))
        {
            var cursorParts = cursor.Split('|');
            if (cursorParts.Length == 2 && 
                int.TryParse(cursorParts[0], out var cursorOrder) &&
                int.TryParse(cursorParts[1], out var cursorId))
            {
                query = query.Where(l => 
                    l.Order > cursorOrder || 
                    (l.Order == cursorOrder && l.Id > cursorId))
                    .OrderBy(l => l.Order)
                    .ThenBy(l => l.Id);
            }
        }

        var locations = await query
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = locations.Count > limit;
        var actualLocations = hasMore ? locations.Take(limit).ToList() : locations;
        
        string? nextCursor = null;
        if (hasMore && actualLocations.Count > 0)
        {
            var lastLocation = actualLocations.Last();
            nextCursor = $"{lastLocation.Order}|{lastLocation.Id}";
        }

        return new PaginatedResponse<Location>
        {
            Data = actualLocations,
            NextCursor = nextCursor,
            HasMore = hasMore,
            Count = actualLocations.Count,
            Limit = limit
        };
    }
}