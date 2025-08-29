using Domain.Data;
using Domain.DTOs.Common;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class FishingReportRepository : IFishingReportRepository
{
    private readonly AppDbContext _context;

    public FishingReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FishingReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FishingReports
            .Include(fr => fr.Location)
            .FirstOrDefaultAsync(fr => fr.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FishingReport>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.FishingReports
            .Include(fr => fr.Location)
            .Where(fr => fr.AccountId == accountId)
            .OrderByDescending(fr => fr.TripDate)
            .ThenByDescending(fr => fr.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<FishingReport>> GetByAccountIdPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        var query = _context.FishingReports
            .Include(fr => fr.Location)
            .Where(fr => fr.AccountId == accountId)
            .OrderByDescending(fr => fr.TripDate)
            .ThenByDescending(fr => fr.CreatedDate);

        if (!string.IsNullOrEmpty(cursor))
        {
            var cursorParts = cursor.Split('|');
            if (cursorParts.Length == 2 && 
                DateTime.TryParse(cursorParts[0], out var cursorTripDate) &&
                DateTime.TryParse(cursorParts[1], out var cursorCreatedDate))
            {
                query = query.Where(fr => 
                    fr.TripDate < cursorTripDate || 
                    (fr.TripDate == cursorTripDate && fr.CreatedDate < cursorCreatedDate))
                    .OrderByDescending(fr => fr.TripDate)
                    .ThenByDescending(fr => fr.CreatedDate);
            }
        }

        var reports = await query
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = reports.Count > limit;
        var actualReports = hasMore ? reports.Take(limit).ToList() : reports;
        
        string? nextCursor = null;
        if (hasMore && actualReports.Count > 0)
        {
            var lastReport = actualReports.Last();
            nextCursor = $"{lastReport.TripDate:O}|{lastReport.CreatedDate:O}";
        }

        return new PaginatedResponse<FishingReport>
        {
            Data = actualReports,
            NextCursor = nextCursor,
            HasMore = hasMore,
            Count = actualReports.Count
        };
    }

    public async Task<FishingReport> AddAsync(FishingReport fishingReport, CancellationToken cancellationToken = default)
    {
        _context.FishingReports.Add(fishingReport);
        await _context.SaveChangesAsync(cancellationToken);
        return fishingReport;
    }

    public async Task UpdateAsync(FishingReport fishingReport, CancellationToken cancellationToken = default)
    {
        _context.FishingReports.Update(fishingReport);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var fishingReport = await _context.FishingReports
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(fr => fr.Id == id, cancellationToken);
        
        if (fishingReport != null)
        {
            fishingReport.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FishingReports
            .AnyAsync(fr => fr.Id == id, cancellationToken);
    }

    public async Task<bool> BelongsToAccountAsync(Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.FishingReports
            .AnyAsync(fr => fr.Id == fishingReportId && fr.AccountId == accountId, cancellationToken);
    }
}