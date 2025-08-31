using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Landing
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public int FishSpeciesId { get; set; }

    [Required]
    public Guid FishingReportId { get; set; }

    public decimal? LengthInInches { get; set; }

    public Guid? LureUsed { get; set; }

    public Guid? RodUsed { get; set; }

    public Guid? ReelUsed { get; set; }

    public int? MainLineTestInPounds { get; set; }

    public int? LeaderLineTestInPounds { get; set; }

    public DateTime? TimeOfCatch { get; set; }

    public bool Released { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedDate { get; set; }

    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

    public bool IsDeleted => DeletedDate.HasValue;

    // Navigation properties
    public Account Account { get; set; } = null!;
    public FishSpecies FishSpecies { get; set; } = null!;
    public FishingReport FishingReport { get; set; } = null!;
    public Tackle? Lure { get; set; }
    public Tackle? Rod { get; set; }
    public Tackle? Reel { get; set; }
}