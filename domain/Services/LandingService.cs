using Domain.DTOs.Common;
using Domain.DTOs.FishSpecies;
using Domain.DTOs.Landing;
using Domain.DTOs.Tackle;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class LandingService : ILandingService
{
    private readonly ILandingRepository _landingRepository;
    private readonly IFishingReportRepository _fishingReportRepository;
    private readonly IAccountRepository _accountRepository;

    public LandingService(ILandingRepository landingRepository, IFishingReportRepository fishingReportRepository, IAccountRepository accountRepository)
    {
        _landingRepository = landingRepository;
        _fishingReportRepository = fishingReportRepository;
        _accountRepository = accountRepository;
    }

    public async Task<LandingDto?> GetLandingByIdAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        if (!await _fishingReportRepository.BelongsToAccountAsync(fishingReportId, accountId, cancellationToken))
        {
            return null;
        }

        var landing = await _landingRepository.GetByIdAsync(id, fishingReportId, accountId, cancellationToken);
        return landing != null ? MapToDto(landing) : null;
    }

    public async Task<PaginatedResponse<LandingDto>> GetLandingsByFishingReportIdAsync(Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return await GetLandingsByFishingReportIdPaginatedAsync(fishingReportId, accountId, 25, null, cancellationToken);
    }

    public async Task<PaginatedResponse<LandingDto>> GetLandingsByFishingReportIdPaginatedAsync(Guid fishingReportId, Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        if (!await _fishingReportRepository.BelongsToAccountAsync(fishingReportId, accountId, cancellationToken))
        {
            return new PaginatedResponse<LandingDto>
            {
                Data = new List<LandingDto>(),
                NextCursor = null,
                HasMore = false,
                Count = 0,
                Limit = limit
            };
        }

        var (landings, nextCursor) = await _landingRepository.GetByFishingReportIdPaginatedAsync(fishingReportId, accountId, limit, cursor, cancellationToken);
        var landingsList = landings.ToList();
        
        return new PaginatedResponse<LandingDto>
        {
            Data = landingsList.Select(MapToDto),
            NextCursor = nextCursor,
            HasMore = !string.IsNullOrEmpty(nextCursor),
            Count = landingsList.Count,
            Limit = limit
        };
    }

    public async Task<LandingDto> CreateLandingAsync(CreateLandingDto createLandingDto, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        if (!await _accountRepository.ExistsAsync(accountId, cancellationToken))
        {
            throw new InvalidOperationException("Account does not exist");
        }

        if (!await _fishingReportRepository.BelongsToAccountAsync(fishingReportId, accountId, cancellationToken))
        {
            throw new InvalidOperationException("Fishing report does not exist or does not belong to the account");
        }

        var landing = new Landing
        {
            AccountId = accountId,
            FishSpeciesId = createLandingDto.FishSpeciesId,
            FishingReportId = fishingReportId,
            LengthInInches = createLandingDto.LengthInInches,
            LureUsed = createLandingDto.LureUsed,
            RodUsed = createLandingDto.RodUsed,
            ReelUsed = createLandingDto.ReelUsed,
            MainLineTestInPounds = createLandingDto.MainLineTestInPounds,
            LeaderLineTestInPounds = createLandingDto.LeaderLineTestInPounds,
            TimeOfCatch = createLandingDto.TimeOfCatch,
            Released = createLandingDto.Released
        };

        var createdLanding = await _landingRepository.AddAsync(landing, cancellationToken);
        return MapToDto(createdLanding);
    }

    public async Task<LandingDto?> UpdateLandingAsync(Guid id, UpdateLandingDto updateLandingDto, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        var existingLanding = await _landingRepository.GetByIdAsync(id, fishingReportId, accountId, cancellationToken);
        
        if (existingLanding == null)
        {
            return null;
        }

        existingLanding.FishSpeciesId = updateLandingDto.FishSpeciesId;
        existingLanding.LengthInInches = updateLandingDto.LengthInInches;
        existingLanding.LureUsed = updateLandingDto.LureUsed;
        existingLanding.RodUsed = updateLandingDto.RodUsed;
        existingLanding.ReelUsed = updateLandingDto.ReelUsed;
        existingLanding.MainLineTestInPounds = updateLandingDto.MainLineTestInPounds;
        existingLanding.LeaderLineTestInPounds = updateLandingDto.LeaderLineTestInPounds;
        existingLanding.TimeOfCatch = updateLandingDto.TimeOfCatch;
        existingLanding.Released = updateLandingDto.Released;
        existingLanding.UpdatedDate = DateTime.UtcNow;

        var updatedLanding = await _landingRepository.UpdateAsync(existingLanding, cancellationToken);
        return MapToDto(updatedLanding);
    }

    public async Task<bool> DeleteLandingAsync(Guid id, Guid fishingReportId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _landingRepository.DeleteAsync(id, fishingReportId, accountId, cancellationToken);
    }

    private static LandingDto MapToDto(Landing landing)
    {
        return new LandingDto
        {
            Id = landing.Id,
            AccountId = landing.AccountId,
            FishSpeciesId = landing.FishSpeciesId,
            FishingReportId = landing.FishingReportId,
            LengthInInches = landing.LengthInInches,
            LureUsed = landing.LureUsed,
            RodUsed = landing.RodUsed,
            ReelUsed = landing.ReelUsed,
            MainLineTestInPounds = landing.MainLineTestInPounds,
            LeaderLineTestInPounds = landing.LeaderLineTestInPounds,
            TimeOfCatch = landing.TimeOfCatch,
            Released = landing.Released,
            CreatedDate = landing.CreatedDate,
            UpdatedDate = landing.UpdatedDate,
            FishSpecies = landing.FishSpecies != null ? new FishSpeciesDto
            {
                Id = landing.FishSpecies.Id,
                Order = landing.FishSpecies.Order,
                Name = landing.FishSpecies.Name,
                ScientificName = landing.FishSpecies.ScientificName,
                Description = landing.FishSpecies.Description,
                CreatedDate = landing.FishSpecies.CreatedDate
            } : null,
            Lure = landing.Lure != null ? new TackleDto
            {
                Id = landing.Lure.Id,
                AccountId = landing.Lure.AccountId,
                Type = landing.Lure.Type,
                Name = landing.Lure.Name,
                Description = landing.Lure.Description,
                CreatedDate = landing.Lure.CreatedDate,
                UpdatedDate = landing.Lure.UpdatedDate
            } : null,
            Rod = landing.Rod != null ? new TackleDto
            {
                Id = landing.Rod.Id,
                AccountId = landing.Rod.AccountId,
                Type = landing.Rod.Type,
                Name = landing.Rod.Name,
                Description = landing.Rod.Description,
                CreatedDate = landing.Rod.CreatedDate,
                UpdatedDate = landing.Rod.UpdatedDate
            } : null,
            Reel = landing.Reel != null ? new TackleDto
            {
                Id = landing.Reel.Id,
                AccountId = landing.Reel.AccountId,
                Type = landing.Reel.Type,
                Name = landing.Reel.Name,
                Description = landing.Reel.Description,
                CreatedDate = landing.Reel.CreatedDate,
                UpdatedDate = landing.Reel.UpdatedDate
            } : null
        };
    }
}