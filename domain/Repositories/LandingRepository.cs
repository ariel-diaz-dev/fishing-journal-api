using Domain.Data;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class LandingRepository : ILandingRepository
{
    private readonly AppDbContext _context;

    public LandingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Landing?> GetByIdAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Landings
            .Include(l => l.FishSpecies)
            .Include(l => l.Lure)
            .Include(l => l.Rod)
            .Include(l => l.Reel)
            .FirstOrDefaultAsync(l => l.Id == id && l.FishingReportId == fishingReportId && l.AccountId == accountId, cancellationToken);
    }

    public async Task<IEnumerable<Landing>> GetByFishingReportIdAsync(Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Landings
            .Include(l => l.FishSpecies)
            .Include(l => l.Lure)
            .Include(l => l.Rod)
            .Include(l => l.Reel)
            .Where(l => l.FishingReportId == fishingReportId && l.AccountId == accountId)
            .OrderBy(l => l.LengthInInches)
            .ThenByDescending(l => l.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Landing> Items, string? NextCursor)> GetByFishingReportIdPaginatedAsync(Guid fishingReportId, Guid accountId, int limit, string? cursor = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Landings
            .Include(l => l.FishSpecies)
            .Include(l => l.Lure)
            .Include(l => l.Rod)
            .Include(l => l.Reel)
            .Where(l => l.FishingReportId == fishingReportId && l.AccountId == accountId)
            .OrderBy(l => l.LengthInInches)
            .ThenByDescending(l => l.CreatedDate);

        if (!string.IsNullOrEmpty(cursor))
        {
            var cursorParts = cursor.Split('|');
            if (cursorParts.Length == 2 && 
                decimal.TryParse(cursorParts[0], out var cursorLength) &&
                DateTime.TryParse(cursorParts[1], out var cursorCreatedDate))
            {
                query = query.Where(l => 
                    l.LengthInInches > cursorLength || 
                    (l.LengthInInches == cursorLength && l.CreatedDate < cursorCreatedDate))
                    .OrderBy(l => l.LengthInInches)
                    .ThenByDescending(l => l.CreatedDate);
            }
        }

        var landings = await query
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = landings.Count > limit;
        var actualLandings = hasMore ? landings.Take(limit).ToList() : landings;
        
        string? nextCursor = null;
        if (hasMore && actualLandings.Count > 0)
        {
            var lastLanding = actualLandings.Last();
            nextCursor = $"{lastLanding.LengthInInches}|{lastLanding.CreatedDate:O}";
        }

        return (actualLandings, nextCursor);
    }

    public async Task<Landing> AddAsync(Landing landing, CancellationToken cancellationToken = default)
    {
        _context.Landings.Add(landing);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Reload with includes
        return await GetByIdAsync(landing.Id, landing.FishingReportId, landing.AccountId, cancellationToken) ?? landing;
    }

    public async Task<Landing> UpdateAsync(Landing landing, CancellationToken cancellationToken = default)
    {
        _context.Landings.Update(landing);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Reload with includes
        return await GetByIdAsync(landing.Id, landing.FishingReportId, landing.AccountId, cancellationToken) ?? landing;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        var landing = await _context.Landings
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.Id == id && l.FishingReportId == fishingReportId && l.AccountId == accountId, cancellationToken);
        
        if (landing != null)
        {
            landing.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        
        return false;
    }
}