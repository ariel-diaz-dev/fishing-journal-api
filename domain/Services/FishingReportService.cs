using Domain.DTOs.Common;
using Domain.DTOs.FishingReport;
using Domain.DTOs.Location;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class FishingReportService : IFishingReportService
{
    private readonly IFishingReportRepository _fishingReportRepository;
    private readonly IAccountRepository _accountRepository;

    public FishingReportService(IFishingReportRepository fishingReportRepository, IAccountRepository accountRepository)
    {
        _fishingReportRepository = fishingReportRepository;
        _accountRepository = accountRepository;
    }

    public async Task<FishingReportDto?> GetFishingReportByIdAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default)
    {
        var fishingReport = await _fishingReportRepository.GetByIdAsync(id, cancellationToken);
        
        if (fishingReport == null || fishingReport.AccountId != accountId)
        {
            return null;
        }

        return MapToDto(fishingReport);
    }

    public async Task<IEnumerable<FishingReportDto>> GetFishingReportsByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var fishingReports = await _fishingReportRepository.GetByAccountIdAsync(accountId, cancellationToken);
        return fishingReports.Select(MapToDto);
    }

    public async Task<PaginatedResponse<FishingReportDto>> GetFishingReportsByAccountIdPaginatedAsync(Guid accountId, int limit = 25, string? cursor = null, CancellationToken cancellationToken = default)
    {
        var paginatedReports = await _fishingReportRepository.GetByAccountIdPaginatedAsync(accountId, limit, cursor, cancellationToken);
        
        return new PaginatedResponse<FishingReportDto>
        {
            Data = paginatedReports.Data.Select(MapToDto),
            NextCursor = paginatedReports.NextCursor,
            HasMore = paginatedReports.HasMore,
            Count = paginatedReports.Count
        };
    }

    public async Task<FishingReportDto> CreateFishingReportAsync(CreateFishingReportDto createFishingReportDto, Guid accountId, CancellationToken cancellationToken = default)
    {
        if (!await _accountRepository.ExistsAsync(accountId, cancellationToken))
        {
            throw new InvalidOperationException("Account does not exist");
        }

        var fishingReport = new FishingReport
        {
            AccountId = accountId,
            LocationId = createFishingReportDto.LocationId,
            ArrivalTime = createFishingReportDto.ArrivalTime,
            DepartureTime = createFishingReportDto.DepartureTime,
            FirstHighTide = createFishingReportDto.FirstHighTide,
            SecondHighTide = createFishingReportDto.SecondHighTide,
            FirstLowTide = createFishingReportDto.FirstLowTide,
            SecondLowTide = createFishingReportDto.SecondLowTide,
            DaytimeTemperature = createFishingReportDto.DaytimeTemperature,
            WaterTemperature = createFishingReportDto.WaterTemperature,
            WindSpeedInMilesPerHour = createFishingReportDto.WindSpeedInMilesPerHour,
            WindDirection = createFishingReportDto.WindDirection,
            WeatherConditions = createFishingReportDto.WeatherConditions,
            Notes = createFishingReportDto.Notes,
            TripDate = createFishingReportDto.TripDate
        };

        var createdReport = await _fishingReportRepository.AddAsync(fishingReport, cancellationToken);
        var result = await _fishingReportRepository.GetByIdAsync(createdReport.Id, cancellationToken);
        return MapToDto(result!);
    }

    public async Task<FishingReportDto?> UpdateFishingReportAsync(Guid id, UpdateFishingReportDto updateFishingReportDto, Guid accountId, CancellationToken cancellationToken = default)
    {
        var existingReport = await _fishingReportRepository.GetByIdAsync(id, cancellationToken);
        
        if (existingReport == null || existingReport.AccountId != accountId)
        {
            return null;
        }

        if (updateFishingReportDto.LocationId.HasValue)
            existingReport.LocationId = updateFishingReportDto.LocationId.Value;
        if (updateFishingReportDto.ArrivalTime.HasValue)
            existingReport.ArrivalTime = updateFishingReportDto.ArrivalTime;
        if (updateFishingReportDto.DepartureTime.HasValue)
            existingReport.DepartureTime = updateFishingReportDto.DepartureTime;
        if (updateFishingReportDto.FirstHighTide.HasValue)
            existingReport.FirstHighTide = updateFishingReportDto.FirstHighTide;
        if (updateFishingReportDto.SecondHighTide.HasValue)
            existingReport.SecondHighTide = updateFishingReportDto.SecondHighTide;
        if (updateFishingReportDto.FirstLowTide.HasValue)
            existingReport.FirstLowTide = updateFishingReportDto.FirstLowTide;
        if (updateFishingReportDto.SecondLowTide.HasValue)
            existingReport.SecondLowTide = updateFishingReportDto.SecondLowTide;
        if (updateFishingReportDto.DaytimeTemperature.HasValue)
            existingReport.DaytimeTemperature = updateFishingReportDto.DaytimeTemperature;
        if (updateFishingReportDto.WaterTemperature.HasValue)
            existingReport.WaterTemperature = updateFishingReportDto.WaterTemperature;
        if (updateFishingReportDto.WindSpeedInMilesPerHour.HasValue)
            existingReport.WindSpeedInMilesPerHour = updateFishingReportDto.WindSpeedInMilesPerHour;
        if (updateFishingReportDto.WindDirection != null)
            existingReport.WindDirection = updateFishingReportDto.WindDirection;
        if (updateFishingReportDto.WeatherConditions.HasValue)
            existingReport.WeatherConditions = updateFishingReportDto.WeatherConditions;
        if (updateFishingReportDto.Notes != null)
            existingReport.Notes = updateFishingReportDto.Notes;
        if (updateFishingReportDto.TripDate.HasValue)
            existingReport.TripDate = updateFishingReportDto.TripDate;

        existingReport.UpdatedDate = DateTime.UtcNow;

        await _fishingReportRepository.UpdateAsync(existingReport, cancellationToken);
        var result = await _fishingReportRepository.GetByIdAsync(existingReport.Id, cancellationToken);
        return MapToDto(result!);
    }

    public async Task<bool> DeleteFishingReportAsync(Guid id, Guid accountId, CancellationToken cancellationToken = default)
    {
        if (!await _fishingReportRepository.BelongsToAccountAsync(id, accountId, cancellationToken))
        {
            return false;
        }

        await _fishingReportRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    private static FishingReportDto MapToDto(FishingReport fishingReport)
    {
        return new FishingReportDto
        {
            Id = fishingReport.Id,
            AccountId = fishingReport.AccountId,
            LocationId = fishingReport.LocationId,
            ArrivalTime = fishingReport.ArrivalTime,
            DepartureTime = fishingReport.DepartureTime,
            FirstHighTide = fishingReport.FirstHighTide,
            SecondHighTide = fishingReport.SecondHighTide,
            FirstLowTide = fishingReport.FirstLowTide,
            SecondLowTide = fishingReport.SecondLowTide,
            DaytimeTemperature = fishingReport.DaytimeTemperature,
            WaterTemperature = fishingReport.WaterTemperature,
            WindSpeedInMilesPerHour = fishingReport.WindSpeedInMilesPerHour,
            WindDirection = fishingReport.WindDirection,
            WeatherConditions = fishingReport.WeatherConditions,
            Notes = fishingReport.Notes,
            TripDate = fishingReport.TripDate,
            CreatedDate = fishingReport.CreatedDate,
            UpdatedDate = fishingReport.UpdatedDate,
            Location = fishingReport.Location != null ? new LocationDto
            {
                Id = fishingReport.Location.Id,
                Order = fishingReport.Location.Order,
                Name = fishingReport.Location.Name,
                Latitude = fishingReport.Location.Latitude,
                Longitude = fishingReport.Location.Longitude,
                Description = fishingReport.Location.Description,
                CreatedDate = fishingReport.Location.CreatedDate
            } : null
        };
    }
}