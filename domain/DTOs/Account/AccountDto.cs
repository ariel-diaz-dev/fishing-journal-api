namespace Domain.DTOs.Account;

public record AccountDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public DateTime? DeletedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}