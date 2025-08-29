using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Domain.Interfaces;
using Domain.Models;
using Domain.DTOs.Tackle;
using Domain.Enums;
using api.Controllers;

namespace Tests.Controllers;

public class TackleControllerTests
{
    private readonly Mock<ITackleService> _mockTackleService;
    private readonly TackleController _controller;
    private readonly Guid _accountId;

    public TackleControllerTests()
    {
        _mockTackleService = new Mock<ITackleService>();
        _controller = new TackleController(_mockTackleService.Object);
        _accountId = Guid.NewGuid();

        // Set up HttpContext to simulate the authenticated account
        var httpContext = new DefaultHttpContext();
        httpContext.Items["AccountId"] = _accountId;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetAllTackle_ReturnsOkWithTackleList()
    {
        // Arrange
        var tackle1 = new Tackle
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            Type = TackleType.Rod,
            Name = "Fishing Rod 1",
            Description = "A good fishing rod",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        var tackle2 = new Tackle
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            Type = TackleType.Reel,
            Name = "Fishing Reel 1",
            Description = "A reliable fishing reel",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        var tackleList = new List<Tackle> { tackle1, tackle2 };

        _mockTackleService.Setup(s => s.GetAllTackleByAccountAsync(_accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tackleList);

        // Act
        var result = await _controller.GetAllTackle(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tackleDtos = Assert.IsAssignableFrom<IEnumerable<TackleDto>>(okResult.Value);
        var tackleArray = tackleDtos.ToArray();
        Assert.Equal(2, tackleArray.Length);
        Assert.Equal(tackle1.Name, tackleArray[0].Name);
        Assert.Equal(tackle2.Name, tackleArray[1].Name);
    }

    [Fact]
    public async Task GetTackleById_ExistingTackle_ReturnsOkWithTackle()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        var tackle = new Tackle
        {
            Id = tackleId,
            AccountId = _accountId,
            Type = TackleType.Lure,
            Name = "Test Lure",
            Description = "A test lure",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockTackleService.Setup(s => s.GetTackleByIdAsync(tackleId, _accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tackle);

        // Act
        var result = await _controller.GetTackleById(tackleId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tackleDto = Assert.IsType<TackleDto>(okResult.Value);
        Assert.Equal(tackleId, tackleDto.Id);
        Assert.Equal("Test Lure", tackleDto.Name);
        Assert.Equal(TackleType.Lure, tackleDto.Type);
    }

    [Fact]
    public async Task GetTackleById_NonExistingTackle_ReturnsNotFound()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        _mockTackleService.Setup(s => s.GetTackleByIdAsync(tackleId, _accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tackle?)null);

        // Act
        var result = await _controller.GetTackleById(tackleId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateTackle_ValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateTackleDto
        {
            Type = TackleType.Terminal,
            Name = "Sharp Hook",
            Description = "A very sharp fishing hook"
        };

        var createdTackle = new Tackle
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            Type = createDto.Type,
            Name = createDto.Name,
            Description = createDto.Description,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockTackleService.Setup(s => s.CreateTackleAsync(_accountId, createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTackle);

        // Act
        var result = await _controller.CreateTackle(createDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var tackleDto = Assert.IsType<TackleDto>(createdResult.Value);
        Assert.Equal(createdTackle.Id, tackleDto.Id);
        Assert.Equal("Sharp Hook", tackleDto.Name);
        Assert.Equal(nameof(_controller.GetTackleById), createdResult.ActionName);
    }

    [Fact]
    public async Task CreateTackle_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateTackleDto();
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.CreateTackle(createDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateTackle_ServiceThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateTackleDto
        {
            Type = TackleType.Rod,
            Name = "Test Rod"
        };

        _mockTackleService.Setup(s => s.CreateTackleAsync(_accountId, createDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Duplicate tackle name"));

        // Act
        var result = await _controller.CreateTackle(createDto, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Duplicate tackle name", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateTackle_ValidDto_ReturnsOkWithUpdatedTackle()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        var updateDto = new UpdateTackleDto
        {
            Name = "Updated Tackle Name",
            Description = "Updated description"
        };

        var updatedTackle = new Tackle
        {
            Id = tackleId,
            AccountId = _accountId,
            Type = TackleType.Other,
            Name = updateDto.Name,
            Description = updateDto.Description,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UpdatedDate = DateTime.UtcNow
        };

        _mockTackleService.Setup(s => s.UpdateTackleAsync(tackleId, _accountId, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedTackle);

        // Act
        var result = await _controller.UpdateTackle(tackleId, updateDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tackleDto = Assert.IsType<TackleDto>(okResult.Value);
        Assert.Equal(tackleId, tackleDto.Id);
        Assert.Equal("Updated Tackle Name", tackleDto.Name);
    }

    [Fact]
    public async Task UpdateTackle_NonExistingTackle_ReturnsNotFound()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        var updateDto = new UpdateTackleDto { Name = "Test" };

        _mockTackleService.Setup(s => s.UpdateTackleAsync(tackleId, _accountId, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tackle?)null);

        // Act
        var result = await _controller.UpdateTackle(tackleId, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateTackle_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        var updateDto = new UpdateTackleDto();
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.UpdateTackle(tackleId, updateDto, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteTackle_ExistingTackle_ReturnsNoContent()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        _mockTackleService.Setup(s => s.DeleteTackleAsync(tackleId, _accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteTackle(tackleId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTackle_NonExistingTackle_ReturnsNotFound()
    {
        // Arrange
        var tackleId = Guid.NewGuid();
        _mockTackleService.Setup(s => s.DeleteTackleAsync(tackleId, _accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteTackle(tackleId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}