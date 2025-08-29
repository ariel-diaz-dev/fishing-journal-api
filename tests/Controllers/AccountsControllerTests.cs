using Microsoft.AspNetCore.Mvc;
using Moq;
using Domain.Interfaces;
using Domain.Models;
using Domain.DTOs.Account;
using Api.Controllers;

namespace Tests.Controllers;

public class AccountsControllerTests
{
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly AccountsController _controller;

    public AccountsControllerTests()
    {
        _mockAccountService = new Mock<IAccountService>();
        _controller = new AccountsController(_mockAccountService.Object);
    }

    [Fact]
    public async Task GetAccount_ExistingId_ReturnsOkWithAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new Account
        {
            Id = accountId,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockAccountService.Setup(s => s.GetAccountByIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _controller.GetAccount(accountId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var accountDto = Assert.IsType<AccountDto>(okResult.Value);
        Assert.Equal(accountId, accountDto.Id);
        Assert.Equal("test@example.com", accountDto.Email);
        Assert.Equal("John", accountDto.FirstName);
        Assert.Equal("Doe", accountDto.LastName);
    }

    [Fact]
    public async Task GetAccount_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _mockAccountService.Setup(s => s.GetAccountByIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        // Act
        var result = await _controller.GetAccount(accountId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateAccount_ValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateAccountDto
        {
            Email = "newuser@example.com",
            FirstName = "Jane",
            LastName = "Smith"
        };

        var createdAccount = new Account
        {
            Id = Guid.NewGuid(),
            Email = createDto.Email,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockAccountService.Setup(s => s.CreateAccountAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdAccount);

        // Act
        var result = await _controller.CreateAccount(createDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var accountDto = Assert.IsType<AccountDto>(createdResult.Value);
        Assert.Equal(createdAccount.Id, accountDto.Id);
        Assert.Equal("newuser@example.com", accountDto.Email);
        Assert.Equal(nameof(_controller.GetAccount), createdResult.ActionName);
    }

    [Fact]
    public async Task CreateAccount_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateAccountDto();
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _controller.CreateAccount(createDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateAccount_ValidDto_ReturnsOkWithUpdatedAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var updateDto = new UpdateAccountDto
        {
            FirstName = "UpdatedName",
            LastName = "UpdatedLastName"
        };

        var updatedAccount = new Account
        {
            Id = accountId,
            Email = "test@example.com",
            FirstName = updateDto.FirstName,
            LastName = updateDto.LastName,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UpdatedDate = DateTime.UtcNow
        };

        _mockAccountService.Setup(s => s.UpdateAccountAsync(accountId, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedAccount);

        // Act
        var result = await _controller.UpdateAccount(accountId, updateDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var accountDto = Assert.IsType<AccountDto>(okResult.Value);
        Assert.Equal(accountId, accountDto.Id);
        Assert.Equal("UpdatedName", accountDto.FirstName);
        Assert.Equal("UpdatedLastName", accountDto.LastName);
    }

    [Fact]
    public async Task UpdateAccount_NonExistingAccount_ReturnsNotFound()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var updateDto = new UpdateAccountDto { FirstName = "Test" };

        _mockAccountService.Setup(s => s.UpdateAccountAsync(accountId, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        // Act
        var result = await _controller.UpdateAccount(accountId, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateAccount_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var updateDto = new UpdateAccountDto();
        _controller.ModelState.AddModelError("FirstName", "FirstName is required");

        // Act
        var result = await _controller.UpdateAccount(accountId, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteAccount_ExistingAccount_ReturnsNoContent()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _mockAccountService.Setup(s => s.DeleteAccountAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteAccount(accountId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAccount_NonExistingAccount_ReturnsNotFound()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _mockAccountService.Setup(s => s.DeleteAccountAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteAccount(accountId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}