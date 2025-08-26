using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public UserRole UserRole { get; set; }

    [Required]
    public NotificationMethod PreferredNotificationMethod { get; set; } = NotificationMethod.Email;

    public bool EmailNotificationsEnabled { get; set; } = true;

    public bool SmsNotificationsEnabled { get; set; } = true;

    public DateTime? NotificationOptOutDate { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    [EmailAddress]
    public string AlternateEmail { get; set; } = string.Empty;

    [MaxLength(50)]
    public string AlternatePhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string EmergencyContactName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string EmergencyContactPhone { get; set; } = string.Empty;

    [Required]
    public UserStatus Status { get; set; } = UserStatus.Active;

    [Required]
    public Language PreferredLanguage { get; set; } = Language.English;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted => DeletedAt.HasValue;
}