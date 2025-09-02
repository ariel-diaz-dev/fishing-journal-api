using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.FishingReport;

public record CreateFishingReportDto
{
    [Required]
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
    
    [MaxLength(10)]
    public string? WindDirection { get; init; }
    
    public WeatherConditions? WeatherConditions { get; init; }
    
    [MaxLength(2000)]
    public string? Notes { get; init; }
    
    [MaxLength(500)]
    public string? VideoUrl { get; init; }
    
    public DateTime? TripDate { get; init; }
}