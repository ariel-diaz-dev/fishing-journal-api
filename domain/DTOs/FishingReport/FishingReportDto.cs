using Domain.DTOs.Location;
using Domain.Enums;

namespace Domain.DTOs.FishingReport;

public record FishingReportDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public int LocationId { get; init; }
    public DateTime? ArrivalTime { get; init; }
    public DateTime? DepartureTime { get; init; }
    public DateTime? FirstHighTide { get; init; }
    public DateTime? SecondHighTide { get; init; }
    public DateTime? FirstLowTide { get; init; }
    public DateTime? SecondLowTide { get; init; }
    public decimal? DaytimeTemperature { get; init; }
    public decimal? WaterTemperature { get; init; }
    public int? WindSpeedInMilesPerHour { get; init; }
    public string? WindDirection { get; init; }
    public WeatherConditions? WeatherConditions { get; init; }
    public string? Notes { get; init; }
    public string? VideoUrl { get; init; }
    public DateTime? TripDate { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public LocationDto? Location { get; init; }
}