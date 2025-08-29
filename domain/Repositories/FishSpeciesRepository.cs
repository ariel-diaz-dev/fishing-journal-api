using Domain.Data;
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
}