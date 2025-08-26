using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedDate { get; set; }

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public bool IsDeleted => DeletedDate.HasValue;
}