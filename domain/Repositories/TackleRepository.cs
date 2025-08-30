using Domain.Data;
using Domain.DTOs.Common;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class TackleRepository : ITackleRepository
{
    private readonly AppDbContext _context;

    public TackleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tackle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tackle
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Tackle>> GetAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Tackle
            .Where(t => t.AccountId == accountId)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<Tackle>> GetAllByAccountIdPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Tackle
            .Where(t => t.AccountId == accountId)
            .OrderBy(t => t.Name)
            .ThenBy(t => t.Id);

        if (!string.IsNullOrEmpty(cursor))
        {
            var cursorParts = cursor.Split('|');
            if (cursorParts.Length == 2 && 
                Guid.TryParse(cursorParts[1], out var cursorId))
            {
                var cursorName = cursorParts[0];
                query = query.Where(t => 
                    string.Compare(t.Name, cursorName) > 0 || 
                    (t.Name == cursorName && t.Id.CompareTo(cursorId) > 0))
                    .OrderBy(t => t.Name)
                    .ThenBy(t => t.Id);
            }
        }

        var tackle = await query
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = tackle.Count > limit;
        var actualTackle = hasMore ? tackle.Take(limit).ToList() : tackle;
        
        string? nextCursor = null;
        if (hasMore && actualTackle.Count > 0)
        {
            var lastTackle = actualTackle.Last();
            nextCursor = $"{lastTackle.Name}|{lastTackle.Id}";
        }

        return new PaginatedResponse<Tackle>
        {
            Data = actualTackle,
            NextCursor = nextCursor,
            HasMore = hasMore,
            Count = actualTackle.Count,
            Limit = limit
        };
    }

    public async Task<Tackle> AddAsync(Tackle tackle, CancellationToken cancellationToken = default)
    {
        _context.Tackle.Add(tackle);
        await _context.SaveChangesAsync(cancellationToken);
        return tackle;
    }

    public async Task UpdateAsync(Tackle tackle, CancellationToken cancellationToken = default)
    {
        _context.Tackle.Update(tackle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tackle = await _context.Tackle
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        if (tackle != null)
        {
            tackle.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tackle
            .AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<bool> BelongsToAccountAsync(Guid tackleId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Tackle
            .AnyAsync(t => t.Id == tackleId && t.AccountId == accountId, cancellationToken);
    }
}