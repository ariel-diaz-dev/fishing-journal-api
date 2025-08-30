using Domain.DTOs.Common;
using Domain.DTOs.Tackle;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class TackleService : ITackleService
{
    private readonly ITackleRepository _tackleRepository;
    private readonly IAccountRepository _accountRepository;

    public TackleService(ITackleRepository tackleRepository, IAccountRepository accountRepository)
    {
        _tackleRepository = tackleRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Tackle?> GetTackleByIdAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default)
    {
        var tackle = await _tackleRepository.GetByIdAsync(id, cancellationToken);
        
        if (tackle == null || tackle.AccountId != accountId)
        {
            return null;
        }

        return tackle;
    }

    public async Task<PaginatedResponse<Tackle>> GetAllTackleByAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _tackleRepository.GetAllByAccountIdPaginatedAsync(accountId, 25, null, cancellationToken);
    }

    public async Task<PaginatedResponse<Tackle>> GetAllTackleByAccountPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        return await _tackleRepository.GetAllByAccountIdPaginatedAsync(accountId, limit, cursor, cancellationToken);
    }

    public async Task<Tackle> CreateTackleAsync(Guid accountId, CreateTackleDto createTackleDto, CancellationToken cancellationToken = default)
    {
        if (!await _accountRepository.ExistsAsync(accountId, cancellationToken))
        {
            throw new InvalidOperationException("Account does not exist");
        }

        var tackle = new Tackle
        {
            AccountId = accountId,
            Type = createTackleDto.Type,
            Name = createTackleDto.Name,
            Description = createTackleDto.Description
        };

        return await _tackleRepository.AddAsync(tackle, cancellationToken);
    }

    public async Task<Tackle?> UpdateTackleAsync(Guid id, Guid accountId, UpdateTackleDto updateTackleDto, CancellationToken cancellationToken = default)
    {
        var existingTackle = await _tackleRepository.GetByIdAsync(id, cancellationToken);
        
        if (existingTackle == null || existingTackle.AccountId != accountId)
        {
            return null;
        }

        existingTackle.Type = updateTackleDto.Type;
        existingTackle.Name = updateTackleDto.Name;
        existingTackle.Description = updateTackleDto.Description;
        existingTackle.UpdatedDate = DateTime.UtcNow;

        await _tackleRepository.UpdateAsync(existingTackle, cancellationToken);
        return existingTackle;
    }

    public async Task<bool> DeleteTackleAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default)
    {
        if (!await _tackleRepository.BelongsToAccountAsync(id, accountId, cancellationToken))
        {
            return false;
        }

        await _tackleRepository.DeleteAsync(id, cancellationToken);
        return true;
    }
}