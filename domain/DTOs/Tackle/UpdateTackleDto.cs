using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.Tackle;

public record UpdateTackleDto
{
    [Required]
    public TackleType Type { get; init; }

    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; init; } = string.Empty;
}