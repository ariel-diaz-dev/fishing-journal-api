using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Domain.DTOs.Account;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        if (account == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(account));
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountDto createAccountDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var account = await _accountService.CreateAccountAsync(createAccountDto, cancellationToken);

        var accountDto = MapToDto(account);
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, accountDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AccountDto>> UpdateAccount(Guid id, [FromBody] UpdateAccountDto updateAccountDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var account = await _accountService.UpdateAccountAsync(id, updateAccountDto, cancellationToken);

        if (account == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(account));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _accountService.DeleteAccountAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static AccountDto MapToDto(Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            Email = account.Email,
            FirstName = account.FirstName,
            LastName = account.LastName,
            CreatedDate = account.CreatedDate,
            DeletedDate = account.DeletedDate,
            UpdatedDate = account.UpdatedDate
        };
    }
}