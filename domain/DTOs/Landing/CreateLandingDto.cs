using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Landing;

public record CreateLandingDto
{
    [Required]
    public int FishSpeciesId { get; init; }
    
    public decimal? LengthInInches { get; init; }
    public Guid? LureUsed { get; init; }
    public Guid? RodUsed { get; init; }
    public Guid? ReelUsed { get; init; }
    public int? MainLineTestInPounds { get; init; }
    public int? LeaderLineTestInPounds { get; init; }
    public DateTime? TimeOfCatch { get; init; }
    public bool Released { get; init; } = true;
}