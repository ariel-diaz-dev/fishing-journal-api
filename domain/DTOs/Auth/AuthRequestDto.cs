using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Auth;

public record AuthRequestDto
{
    [Required]
    public Guid AccountId { get; init; }
}