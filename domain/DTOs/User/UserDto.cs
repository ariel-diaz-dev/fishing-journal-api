using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.DTOs.User;

public record UserDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole UserRole { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationMethod PreferredNotificationMethod { get; init; }
    public bool EmailNotificationsEnabled { get; init; }
    public bool SmsNotificationsEnabled { get; init; }
    public DateTime? NotificationOptOutDate { get; init; }
    public string Email { get; init; } = string.Empty;
    public string AlternateEmail { get; init; } = string.Empty;
    public string AlternatePhoneNumber { get; init; } = string.Empty;
    public string EmergencyContactName { get; init; } = string.Empty;
    public string EmergencyContactPhone { get; init; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus Status { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Language PreferredLanguage { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}