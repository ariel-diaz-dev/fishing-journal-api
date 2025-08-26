using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.DTOs.User;

public record UpdateUserDto
{
    [Required]
    public Guid AccountId { get; init; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Phone]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole UserRole { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationMethod PreferredNotificationMethod { get; init; } = NotificationMethod.Email;

    public bool EmailNotificationsEnabled { get; init; } = true;

    public bool SmsNotificationsEnabled { get; init; } = true;

    public DateTime? NotificationOptOutDate { get; init; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [MaxLength(100)]
    [EmailAddress]
    public string AlternateEmail { get; init; } = string.Empty;

    [MaxLength(50)]
    public string AlternatePhoneNumber { get; init; } = string.Empty;

    [MaxLength(100)]
    public string EmergencyContactName { get; init; } = string.Empty;

    [MaxLength(50)]
    public string EmergencyContactPhone { get; init; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus Status { get; init; } = UserStatus.Active;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Language PreferredLanguage { get; init; } = Language.English;
}