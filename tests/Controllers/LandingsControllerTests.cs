using api.Controllers;
using Domain.DTOs.Common;
using Domain.DTOs.Landing;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace tests.Controllers;

public class LandingsControllerTests
{
    private readonly Mock<ILandingService> _mockLandingService;
    private readonly LandingsController _controller;
    private readonly Guid _testAccountId = Guid.NewGuid();
    private readonly Guid _testFishingReportId = Guid.NewGuid();

    public LandingsControllerTests()
    {
        _mockLandingService = new Mock<ILandingService>();
        _controller = new LandingsController(_mockLandingService.Object);
        
        SetupControllerContext();
    }

    private void SetupControllerContext()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Items["AccountId"] = _testAccountId;
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetAllLandings_WithoutPagination_ReturnsOkWithLandings()
    {
        // Arrange
        var landings = new List<LandingDto>
        {
            new LandingDto { Id = Guid.NewGuid(), AccountId = _testAccountId, FishingReportId = _testFishingReportId, FishSpeciesId = 1 },
            new LandingDto { Id = Guid.NewGuid(), AccountId = _testAccountId, FishingReportId = _testFishingReportId, FishSpeciesId = 2 }
        };
        
        var paginatedResponse = new PaginatedResponse<LandingDto>
        {
            Data = landings,
            Count = 2,
            HasMore = false,
            NextCursor = null,
            Limit = 25
        };

        _mockLandingService
            .Setup(s => s.GetLandingsByFishingReportIdPaginatedAsync(_testFishingReportId, _testAccountId, 25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllLandings(_testFishingReportId, null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<LandingDto>>(okResult.Value);
        Assert.Equal(2, returnedResponse.Count);
    }

    [Fact]
    public async Task GetAllLandings_WithPagination_ReturnsOkWithPaginatedLandings()
    {
        // Arrange
        var landings = new List<LandingDto>
        {
            new LandingDto { Id = Guid.NewGuid(), AccountId = _testAccountId, FishingReportId = _testFishingReportId, FishSpeciesId = 1 }
        };

        var paginatedResponse = new PaginatedResponse<LandingDto>
        {
            Data = landings,
            Count = 1,
            HasMore = true,
            NextCursor = "next-cursor",
            Limit = 10
        };

        _mockLandingService
            .Setup(s => s.GetLandingsByFishingReportIdPaginatedAsync(_testFishingReportId, _testAccountId, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllLandings(_testFishingReportId, 10, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<LandingDto>>(okResult.Value);
        Assert.Equal(1, returnedResponse.Count);
        Assert.True(returnedResponse.HasMore);
        Assert.Equal("next-cursor", returnedResponse.NextCursor);
    }

    [Fact]
    public async Task GetAllLandings_WithExcessiveLimit_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetAllLandings(_testFishingReportId, 101, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Limit cannot exceed 100", badRequestResult.Value);
    }

    [Fact]
    public async Task GetLandingById_WithValidId_ReturnsOkWithLanding()
    {
        // Arrange
        var landingId = Guid.NewGuid();
        var landing = new LandingDto 
        { 
            Id = landingId, 
            AccountId = _testAccountId,
            FishingReportId = _testFishingReportId,
            FishSpeciesId = 1,
            LengthInInches = 24.5m,
            Released = true
        };
        _mockLandingService
            .Setup(s => s.GetLandingByIdAsync(landingId, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(landing);

        // Act
        var result = await _controller.GetLandingById(_testFishingReportId, landingId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLanding = Assert.IsType<LandingDto>(okResult.Value);
        Assert.Equal(landingId, returnedLanding.Id);
        Assert.Equal(24.5m, returnedLanding.LengthInInches);
    }

    [Fact]
    public async Task GetLandingById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var landingId = Guid.NewGuid();
        _mockLandingService
            .Setup(s => s.GetLandingByIdAsync(landingId, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LandingDto?)null);

        // Act
        var result = await _controller.GetLandingById(_testFishingReportId, landingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateLanding_WithValidDto_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateLandingDto 
        { 
            FishSpeciesId = 1,
            LengthInInches = 20.0m,
            Released = true
        };
        var createdLanding = new LandingDto 
        { 
            Id = Guid.NewGuid(), 
            AccountId = _testAccountId,
            FishingReportId = _testFishingReportId,
            FishSpeciesId = 1,
            LengthInInches = 20.0m,
            Released = true
        };
        _mockLandingService
            .Setup(s => s.CreateLandingAsync(createDto, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdLanding);

        // Act
        var result = await _controller.CreateLanding(_testFishingReportId, createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedLanding = Assert.IsType<LandingDto>(createdResult.Value);
        Assert.Equal(createdLanding.Id, returnedLanding.Id);
        Assert.Equal("GetLandingById", createdResult.ActionName);
    }

    [Fact]
    public async Task CreateLanding_WithInvalidDto_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateLandingDto { FishSpeciesId = 0 }; // Invalid - required field
        _controller.ModelState.AddModelError("FishSpeciesId", "The FishSpeciesId field is required.");

        // Act
        var result = await _controller.CreateLanding(_testFishingReportId, createDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateLanding_WithServiceException_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateLandingDto 
        { 
            FishSpeciesId = 1,
            LengthInInches = 20.0m,
            Released = true
        };
        _mockLandingService
            .Setup(s => s.CreateLandingAsync(createDto, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Fishing report does not exist"));

        // Act
        var result = await _controller.CreateLanding(_testFishingReportId, createDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Fishing report does not exist", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateLanding_WithValidDto_ReturnsOkWithUpdatedLanding()
    {
        // Arrange
        var landingId = Guid.NewGuid();
        var updateDto = new UpdateLandingDto 
        { 
            FishSpeciesId = 2,
            LengthInInches = 22.0m,
            Released = false
        };
        var updatedLanding = new LandingDto 
        { 
            Id = landingId, 
            AccountId = _testAccountId,
            FishingReportId = _testFishingReportId,
            FishSpeciesId = 2,
            LengthInInches = 22.0m,
            Released = false
        };
        _mockLandingService
            .Setup(s => s.UpdateLandingAsync(landingId, updateDto, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedLanding);

        // Act
        var result = await _controller.UpdateLanding(_testFishingReportId, landingId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLanding = Assert.IsType<LandingDto>(okResult.Value);
        Assert.Equal(landingId, returnedLanding.Id);
        Assert.Equal(22.0m, returnedLanding.LengthInInches);
        Assert.False(returnedLanding.Released);
    }

    [Fact]
    public async Task UpdateLanding_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var landingId = Guid.NewGuid();
        var updateDto = new UpdateLandingDto 
        { 
            FishSpeciesId = 2,
            LengthInInches = 22.0m,
            Released = false
        };
        _mockLandingService
            .Setup(s => s.UpdateLandingAsync(landingId, updateDto, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LandingDto?)null);

        // Act
        var result = await _controller.UpdateLanding(_testFishingReportId, landingId, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteLanding_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var landingId = Guid.NewGuid();
        _mockLandingService
            .Setup(s => s.DeleteLandingAsync(landingId, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteLanding(_testFishingReportId, landingId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteLanding_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var landingId = Guid.NewGuid();
        _mockLandingService
            .Setup(s => s.DeleteLandingAsync(landingId, _testFishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteLanding(_testFishingReportId, landingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}