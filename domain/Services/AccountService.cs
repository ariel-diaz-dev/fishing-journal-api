using Domain.DTOs.Account;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _accountRepository.GetByIdAsync(id, cancellationToken);
    }


    public async Task<Account> CreateAccountAsync(CreateAccountDto createAccountDto, CancellationToken cancellationToken = default)
    {
        var account = new Account
        {
            Email = createAccountDto.Email,
            FirstName = createAccountDto.FirstName,
            LastName = createAccountDto.LastName
        };

        return await _accountRepository.AddAsync(account, cancellationToken);
    }

    public async Task<Account?> UpdateAccountAsync(Guid id, UpdateAccountDto updateAccountDto, CancellationToken cancellationToken = default)
    {
        var existingAccount = await _accountRepository.GetByIdAsync(id, cancellationToken);
        if (existingAccount == null)
        {
            return null;
        }

        existingAccount.Email = updateAccountDto.Email;
        existingAccount.FirstName = updateAccountDto.FirstName;
        existingAccount.LastName = updateAccountDto.LastName;
        existingAccount.UpdatedDate = DateTime.UtcNow;

        await _accountRepository.UpdateAsync(existingAccount, cancellationToken);
        return existingAccount;
    }

    public async Task<bool> DeleteAccountAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await _accountRepository.ExistsAsync(id, cancellationToken))
        {
            return false;
        }

        await _accountRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<bool> AccountExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _accountRepository.ExistsAsync(id, cancellationToken);
    }
}