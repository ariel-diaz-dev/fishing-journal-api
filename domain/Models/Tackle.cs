using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Models;

public class Tackle
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AccountId { get; set; }

    [Required]
    public TackleType Type { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedDate { get; set; }

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public bool IsDeleted => DeletedDate.HasValue;

    public Account Account { get; set; } = null!;
}