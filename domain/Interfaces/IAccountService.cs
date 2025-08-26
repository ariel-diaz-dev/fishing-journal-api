using Domain.DTOs.Account;
using Domain.Models;

namespace Domain.Interfaces;

public interface IAccountService
{
    Task<Account?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Account> CreateAccountAsync(CreateAccountDto createAccountDto, CancellationToken cancellationToken = default);
    Task<Account?> UpdateAccountAsync(Guid id, UpdateAccountDto updateAccountDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAccountAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AccountExistsAsync(Guid id, CancellationToken cancellationToken = default);
}