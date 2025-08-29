using api.Controllers;
using Domain.DTOs.Common;
using Domain.DTOs.FishingReport;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Domain.Enums;

namespace tests.Controllers;

public class FishingReportsControllerTests
{
    private readonly Mock<IFishingReportService> _mockFishingReportService;
    private readonly FishingReportsController _controller;
    private readonly Guid _testAccountId = Guid.NewGuid();

    public FishingReportsControllerTests()
    {
        _mockFishingReportService = new Mock<IFishingReportService>();
        _controller = new FishingReportsController(_mockFishingReportService.Object);
        
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
    public async Task GetAllFishingReports_ReturnsOkWithFishingReports()
    {
        // Arrange
        var fishingReports = new List<FishingReportDto>
        {
            new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId },
            new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId }
        };
        _mockFishingReportService
            .Setup(s => s.GetFishingReportsByAccountIdAsync(_testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fishingReports);

        // Act
        var result = await _controller.GetAllFishingReports(null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReports = Assert.IsAssignableFrom<IEnumerable<FishingReportDto>>(okResult.Value);
        Assert.Equal(2, returnedReports.Count());
    }

    [Fact]
    public async Task GetFishingReportById_WithValidId_ReturnsOkWithFishingReport()
    {
        // Arrange
        var fishingReportId = Guid.NewGuid();
        var fishingReport = new FishingReportDto 
        { 
            Id = fishingReportId, 
            AccountId = _testAccountId,
            Notes = "Test fishing report"
        };
        _mockFishingReportService
            .Setup(s => s.GetFishingReportByIdAsync(fishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fishingReport);

        // Act
        var result = await _controller.GetFishingReportById(fishingReportId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReport = Assert.IsType<FishingReportDto>(okResult.Value);
        Assert.Equal(fishingReportId, returnedReport.Id);
        Assert.Equal("Test fishing report", returnedReport.Notes);
    }

    [Fact]
    public async Task GetFishingReportById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var fishingReportId = Guid.NewGuid();
        _mockFishingReportService
            .Setup(s => s.GetFishingReportByIdAsync(fishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FishingReportDto?)null);

        // Act
        var result = await _controller.GetFishingReportById(fishingReportId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateFishingReport_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var createDto = new CreateFishingReportDto
        {
            LocationId = 1,
            Notes = "Great fishing day!",
            WeatherConditions = WeatherConditions.Sunny,
            TripDate = DateTime.Today
        };
        var createdReport = new FishingReportDto
        {
            Id = Guid.NewGuid(),
            AccountId = _testAccountId,
            LocationId = 1,
            Notes = "Great fishing day!",
            WeatherConditions = WeatherConditions.Sunny,
            TripDate = DateTime.Today
        };
        _mockFishingReportService
            .Setup(s => s.CreateFishingReportAsync(createDto, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdReport);

        // Act
        var result = await _controller.CreateFishingReport(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetFishingReportById), createdResult.ActionName);
        Assert.Equal(createdReport.Id, createdResult.RouteValues!["id"]);
        var returnedReport = Assert.IsType<FishingReportDto>(createdResult.Value);
        Assert.Equal("Great fishing day!", returnedReport.Notes);
    }

    [Fact]
    public async Task CreateFishingReport_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateFishingReportDto(); // Missing required LocationId
        _controller.ModelState.AddModelError("LocationId", "The LocationId field is required.");

        // Act
        var result = await _controller.CreateFishingReport(createDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateFishingReport_ServiceThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateFishingReportDto { LocationId = 1 };
        _mockFishingReportService
            .Setup(s => s.CreateFishingReportAsync(createDto, _testAccountId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Account does not exist"));

        // Act
        var result = await _controller.CreateFishingReport(createDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Account does not exist", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateFishingReport_WithValidData_ReturnsOkWithUpdatedReport()
    {
        // Arrange
        var fishingReportId = Guid.NewGuid();
        var updateDto = new UpdateFishingReportDto { Notes = "Updated notes" };
        var updatedReport = new FishingReportDto
        {
            Id = fishingReportId,
            AccountId = _testAccountId,
            Notes = "Updated notes"
        };
        _mockFishingReportService
            .Setup(s => s.UpdateFishingReportAsync(fishingReportId, updateDto, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedReport);

        // Act
        var result = await _controller.UpdateFishingReport(fishingReportId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReport = Assert.IsType<FishingReportDto>(okResult.Value);
        Assert.Equal("Updated notes", returnedReport.Notes);
    }

    [Fact]
    public async Task UpdateFishingReport_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var fishingReportId = Guid.NewGuid();
        var updateDto = new UpdateFishingReportDto { Notes = "Updated notes" };
        _mockFishingReportService
            .Setup(s => s.UpdateFishingReportAsync(fishingReportId, updateDto, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FishingReportDto?)null);

        // Act
        var result = await _controller.UpdateFishingReport(fishingReportId, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteFishingReport_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var fishingReportId = Guid.NewGuid();
        _mockFishingReportService
            .Setup(s => s.DeleteFishingReportAsync(fishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteFishingReport(fishingReportId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteFishingReport_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var fishingReportId = Guid.NewGuid();
        _mockFishingReportService
            .Setup(s => s.DeleteFishingReportAsync(fishingReportId, _testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteFishingReport(fishingReportId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllFishingReports_WithLimitParameter_ReturnsPaginatedResults()
    {
        // Arrange
        var limit = 10;
        var paginatedResponse = new PaginatedResponse<FishingReportDto>
        {
            Data = new List<FishingReportDto>
            {
                new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId },
                new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId }
            },
            NextCursor = "2024-01-01T00:00:00.0000000Z|2024-01-01T12:00:00.0000000Z",
            HasMore = true,
            Count = 2
        };
        
        _mockFishingReportService
            .Setup(s => s.GetFishingReportsByAccountIdPaginatedAsync(_testAccountId, limit, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishingReports(limit, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<FishingReportDto>>(okResult.Value);
        Assert.Equal(2, returnedResponse.Count);
        Assert.True(returnedResponse.HasMore);
        Assert.NotNull(returnedResponse.NextCursor);
    }

    [Fact]
    public async Task GetAllFishingReports_WithNextParameter_ReturnsPaginatedResults()
    {
        // Arrange
        var nextCursor = "2024-01-01T00:00:00.0000000Z|2024-01-01T12:00:00.0000000Z";
        var paginatedResponse = new PaginatedResponse<FishingReportDto>
        {
            Data = new List<FishingReportDto>
            {
                new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId }
            },
            NextCursor = null,
            HasMore = false,
            Count = 1
        };
        
        _mockFishingReportService
            .Setup(s => s.GetFishingReportsByAccountIdPaginatedAsync(_testAccountId, 25, nextCursor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishingReports(null, nextCursor);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<FishingReportDto>>(okResult.Value);
        Assert.Equal(1, returnedResponse.Count);
        Assert.False(returnedResponse.HasMore);
        Assert.Null(returnedResponse.NextCursor);
    }

    [Fact]
    public async Task GetAllFishingReports_WithBothLimitAndNextParameters_ReturnsPaginatedResults()
    {
        // Arrange
        var limit = 5;
        var nextCursor = "2024-01-01T00:00:00.0000000Z|2024-01-01T12:00:00.0000000Z";
        var paginatedResponse = new PaginatedResponse<FishingReportDto>
        {
            Data = new List<FishingReportDto>
            {
                new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId },
                new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId },
                new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId }
            },
            NextCursor = "2024-01-02T00:00:00.0000000Z|2024-01-02T12:00:00.0000000Z",
            HasMore = true,
            Count = 3
        };
        
        _mockFishingReportService
            .Setup(s => s.GetFishingReportsByAccountIdPaginatedAsync(_testAccountId, limit, nextCursor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishingReports(limit, nextCursor);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<PaginatedResponse<FishingReportDto>>(okResult.Value);
        Assert.Equal(3, returnedResponse.Count);
        Assert.True(returnedResponse.HasMore);
        Assert.Equal("2024-01-02T00:00:00.0000000Z|2024-01-02T12:00:00.0000000Z", returnedResponse.NextCursor);
    }

    [Fact]
    public async Task GetAllFishingReports_WithoutPaginationParameters_ReturnsNonPaginatedResults()
    {
        // Arrange
        var fishingReports = new List<FishingReportDto>
        {
            new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId },
            new FishingReportDto { Id = Guid.NewGuid(), AccountId = _testAccountId }
        };
        _mockFishingReportService
            .Setup(s => s.GetFishingReportsByAccountIdAsync(_testAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fishingReports);

        // Act
        var result = await _controller.GetAllFishingReports(null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReports = Assert.IsAssignableFrom<IEnumerable<FishingReportDto>>(okResult.Value);
        Assert.Equal(2, returnedReports.Count());
    }
}