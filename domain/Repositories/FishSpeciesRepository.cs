using Domain.Data;
using Domain.DTOs.Common;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class FishSpeciesRepository : IFishSpeciesRepository
{
    private readonly AppDbContext _context;

    public FishSpeciesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FishSpecies>> GetAllFishSpeciesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FishSpecies
            .OrderBy(fs => fs.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<FishSpecies>> GetAllFishSpeciesPaginatedAsync(int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        var query = _context.FishSpecies
            .OrderBy(fs => fs.Order)
            .ThenBy(fs => fs.Id);

        if (!string.IsNullOrEmpty(cursor))
        {
            var cursorParts = cursor.Split('|');
            if (cursorParts.Length == 2 && 
                int.TryParse(cursorParts[0], out var cursorOrder) &&
                int.TryParse(cursorParts[1], out var cursorId))
            {
                query = query.Where(fs => 
                    fs.Order > cursorOrder || 
                    (fs.Order == cursorOrder && fs.Id > cursorId))
                    .OrderBy(fs => fs.Order)
                    .ThenBy(fs => fs.Id);
            }
        }

        var fishSpecies = await query
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = fishSpecies.Count > limit;
        var actualFishSpecies = hasMore ? fishSpecies.Take(limit).ToList() : fishSpecies;
        
        string? nextCursor = null;
        if (hasMore && actualFishSpecies.Count > 0)
        {
            var lastFishSpecies = actualFishSpecies.Last();
            nextCursor = $"{lastFishSpecies.Order}|{lastFishSpecies.Id}";
        }

        return new PaginatedResponse<FishSpecies>
        {
            Data = actualFishSpecies,
            NextCursor = nextCursor,
            HasMore = hasMore,
            Count = actualFishSpecies.Count,
            Limit = limit
        };
    }
}