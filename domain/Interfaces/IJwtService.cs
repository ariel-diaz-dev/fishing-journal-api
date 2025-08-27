namespace Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid accountId);
    Guid? GetAccountIdFromToken(string token);
}