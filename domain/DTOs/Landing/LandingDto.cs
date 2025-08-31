using Domain.DTOs.FishSpecies;
using Domain.DTOs.Tackle;

namespace Domain.DTOs.Landing;

public record LandingDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public int FishSpeciesId { get; init; }
    public Guid FishingReportId { get; init; }
    public decimal? LengthInInches { get; init; }
    public Guid? LureUsed { get; init; }
    public Guid? RodUsed { get; init; }
    public Guid? ReelUsed { get; init; }
    public int? MainLineTestInPounds { get; init; }
    public int? LeaderLineTestInPounds { get; init; }
    public DateTime? TimeOfCatch { get; init; }
    public bool Released { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public FishSpeciesDto? FishSpecies { get; init; }
    public TackleDto? Lure { get; init; }
    public TackleDto? Rod { get; init; }
    public TackleDto? Reel { get; init; }
}