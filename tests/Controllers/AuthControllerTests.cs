using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Domain.Interfaces;
using Domain.DTOs.Auth;
using Domain.Models;
using api.Controllers;

namespace Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly Mock<IOptions<JwtSettings>> _mockJwtSettings;
    private readonly AuthController _controller;
    private readonly JwtSettings _jwtSettings;

    public AuthControllerTests()
    {
        _mockJwtService = new Mock<IJwtService>();
        _mockAccountService = new Mock<IAccountService>();
        _mockJwtSettings = new Mock<IOptions<JwtSettings>>();
        
        _jwtSettings = new JwtSettings
        {
            Secret = "test-secret-key-that-is-long-enough-for-testing-purposes-and-meets-minimum-requirements",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };
        
        _mockJwtSettings.Setup(x => x.Value).Returns(_jwtSettings);
        
        _controller = new AuthController(_mockJwtService.Object, _mockAccountService.Object, _mockJwtSettings.Object);
    }

    [Fact]
    public async Task GenerateToken_ValidRequest_ReturnsOkWithToken()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var request = new AuthRequestDto { AccountId = accountId };
        var expectedToken = "test-jwt-token";

        _mockAccountService.Setup(s => s.AccountExistsAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockJwtService.Setup(s => s.GenerateToken(accountId))
            .Returns(expectedToken);

        // Act
        var result = await _controller.GenerateToken(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal(expectedToken, response.Token);
        Assert.Equal(accountId, response.AccountId);
        Assert.True(response.ExpiresAt > DateTime.UtcNow);
        Assert.True(response.ExpiresAt <= DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes + 1));
    }

    [Fact]
    public async Task GenerateToken_AccountNotFound_ReturnsBadRequest()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var request = new AuthRequestDto { AccountId = accountId };

        _mockAccountService.Setup(s => s.AccountExistsAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.GenerateToken(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Account not found", badRequestResult.Value);
    }

    [Fact]
    public async Task GenerateToken_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var request = new AuthRequestDto();
        _controller.ModelState.AddModelError("AccountId", "AccountId is required");

        // Act
        var result = await _controller.GenerateToken(request, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GenerateToken_CallsAccountService_WithCorrectParameters()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var request = new AuthRequestDto { AccountId = accountId };
        var cancellationToken = CancellationToken.None;

        _mockAccountService.Setup(s => s.AccountExistsAsync(accountId, cancellationToken))
            .ReturnsAsync(true);

        _mockJwtService.Setup(s => s.GenerateToken(accountId))
            .Returns("test-token");

        // Act
        await _controller.GenerateToken(request, cancellationToken);

        // Assert
        _mockAccountService.Verify(s => s.AccountExistsAsync(accountId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GenerateToken_CallsJwtService_WithCorrectAccountId()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var request = new AuthRequestDto { AccountId = accountId };

        _mockAccountService.Setup(s => s.AccountExistsAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockJwtService.Setup(s => s.GenerateToken(accountId))
            .Returns("test-token");

        // Act
        await _controller.GenerateToken(request, CancellationToken.None);

        // Assert
        _mockJwtService.Verify(s => s.GenerateToken(accountId), Times.Once);
    }
}