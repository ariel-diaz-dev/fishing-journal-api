using Domain.Enums;

namespace Domain.DTOs.Tackle;

public record TackleDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public TackleType Type { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public DateTime? DeletedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}