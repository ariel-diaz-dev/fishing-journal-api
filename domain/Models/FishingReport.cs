using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Models;

public class FishingReport
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public int LocationId { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? FirstHighTide { get; set; }

    public DateTime? SecondHighTide { get; set; }

    public DateTime? FirstLowTide { get; set; }

    public DateTime? SecondLowTide { get; set; }

    public decimal? DaytimeTemperature { get; set; }

    public decimal? WaterTemperature { get; set; }

    public int? WindSpeedInMilesPerHour { get; set; }

    [MaxLength(10)]
    public string? WindDirection { get; set; }

    public WeatherConditions? WeatherConditions { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    public DateTime? TripDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedDate { get; set; }

    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

    public bool IsDeleted => DeletedDate.HasValue;

    // Navigation properties
    public Account Account { get; set; } = null!;
    public Location Location { get; set; } = null!;
}