using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account;

public record CreateAccountDto
{
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; init; } = string.Empty;
}