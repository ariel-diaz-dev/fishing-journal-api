using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Domain.Interfaces;
using Domain.Models;
using Domain.DTOs.Location;
using Api.Controllers;

namespace Tests.Controllers;

public class LocationsControllerTests
{
    private readonly Mock<ILocationService> _mockLocationService;
    private readonly LocationsController _controller;
    private readonly Guid _accountId;

    public LocationsControllerTests()
    {
        _mockLocationService = new Mock<ILocationService>();
        _controller = new LocationsController(_mockLocationService.Object);
        _accountId = Guid.NewGuid();

        // Set up HttpContext to simulate the authenticated account
        // LocationsController extends ControllerBase but doesn't use BaseController's AccountId
        var httpContext = new DefaultHttpContext();
        httpContext.Items["AccountId"] = _accountId;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetAllLocations_ReturnsOkWithLocationsList()
    {
        // Arrange
        var location1 = new Location
        {
            Id = 1,
            Name = "Flamingo, Everglades National Park",
            Latitude = 25.14127m,
            Longitude = -80.92764m,
            Description = "Southern headquarters of Everglades National Park",
            Order = 1,
            CreatedDate = DateTime.UtcNow
        };

        var location2 = new Location
        {
            Id = 2,
            Name = "Card Sound, Florida Keys",
            Latitude = 25.2873m,
            Longitude = -80.3685m,
            Description = "Fishing area near Card Sound Bridge",
            Order = 2,
            CreatedDate = DateTime.UtcNow
        };

        var locations = new List<Location> { location1, location2 };

        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(locations);

        // Act
        var result = await _controller.GetAllLocations(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var locationDtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        var locationArray = locationDtos.ToArray();
        
        Assert.Equal(2, locationArray.Length);
        Assert.Equal(1, locationArray[0].Id);
        Assert.Equal("Flamingo, Everglades National Park", locationArray[0].Name);
        Assert.Equal(25.14127m, locationArray[0].Latitude);
        Assert.Equal(-80.92764m, locationArray[0].Longitude);
        Assert.Equal(1, locationArray[0].Order);
        Assert.Equal("Southern headquarters of Everglades National Park", locationArray[0].Description);
        
        Assert.Equal(2, locationArray[1].Id);
        Assert.Equal("Card Sound, Florida Keys", locationArray[1].Name);
        Assert.Equal(2, locationArray[1].Order);
    }

    [Fact]
    public async Task GetAllLocations_EmptyList_ReturnsOkWithEmptyList()
    {
        // Arrange
        var locations = new List<Location>();
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(locations);

        // Act
        var result = await _controller.GetAllLocations(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var locationDtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        Assert.Empty(locationDtos);
    }

    [Fact]
    public async Task GetAllLocations_ServiceThrowsException_ThrowsException()
    {
        // Arrange
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            _controller.GetAllLocations(CancellationToken.None));
    }

    [Fact]
    public async Task GetAllLocations_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var locations = new List<Location>();
        var cancellationToken = CancellationToken.None;
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(cancellationToken))
            .ReturnsAsync(locations);

        // Act
        await _controller.GetAllLocations(cancellationToken);

        // Assert
        _mockLocationService.Verify(s => s.GetAllLocationsAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAllLocations_MapsAllLocationProperties()
    {
        // Arrange
        var createdDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var location = new Location
        {
            Id = 99,
            Name = "Test Location",
            Latitude = 12.345678m,
            Longitude = -98.765432m,
            Description = "Test description with special chars: @#$%^&*()",
            Order = 5,
            CreatedDate = createdDate
        };

        var locations = new List<Location> { location };
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(locations);

        // Act
        var result = await _controller.GetAllLocations(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var locationDtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        var locationDto = locationDtos.First();
        
        Assert.Equal(location.Id, locationDto.Id);
        Assert.Equal(location.Name, locationDto.Name);
        Assert.Equal(location.Latitude, locationDto.Latitude);
        Assert.Equal(location.Longitude, locationDto.Longitude);
        Assert.Equal(location.Description, locationDto.Description);
        Assert.Equal(location.Order, locationDto.Order);
        Assert.Equal(location.CreatedDate, locationDto.CreatedDate);
    }

    [Fact]
    public async Task GetAllLocations_HandlesNullDescription()
    {
        // Arrange
        var location = new Location
        {
            Id = 1,
            Name = "Location Without Description",
            Latitude = 25.0m,
            Longitude = -80.0m,
            Description = null,
            Order = 1,
            CreatedDate = DateTime.UtcNow
        };

        var locations = new List<Location> { location };
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(locations);

        // Act
        var result = await _controller.GetAllLocations(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var locationDtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        var locationDto = locationDtos.First();
        
        Assert.Null(locationDto.Description);
    }

    [Fact]
    public async Task GetAllLocations_ReturnsLocationsInCorrectOrder()
    {
        // Arrange
        var location1 = new Location { Id = 1, Name = "First", Order = 1, Latitude = 25.0m, Longitude = -80.0m, CreatedDate = DateTime.UtcNow };
        var location2 = new Location { Id = 2, Name = "Second", Order = 2, Latitude = 25.0m, Longitude = -80.0m, CreatedDate = DateTime.UtcNow };
        var location3 = new Location { Id = 3, Name = "Third", Order = 3, Latitude = 25.0m, Longitude = -80.0m, CreatedDate = DateTime.UtcNow };

        // Return them in the order that the repository would (ordered by Order field)
        var locations = new List<Location> { location1, location2, location3 };
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(locations);

        // Act
        var result = await _controller.GetAllLocations(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var locationDtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        var locationArray = locationDtos.ToArray();
        
        Assert.Equal(3, locationArray.Length);
        Assert.Equal(1, locationArray[0].Order);
        Assert.Equal(2, locationArray[1].Order);
        Assert.Equal(3, locationArray[2].Order);
        Assert.Equal("First", locationArray[0].Name);
        Assert.Equal("Second", locationArray[1].Name);
        Assert.Equal("Third", locationArray[2].Name);
    }

    [Fact]
    public async Task GetAllLocations_HandlesDecimalPrecisionCorrectly()
    {
        // Arrange
        var location = new Location
        {
            Id = 1,
            Name = "Precise Location",
            Latitude = 25.12345678m,    // High precision latitude
            Longitude = -80.98765432m,  // High precision longitude
            Order = 1,
            CreatedDate = DateTime.UtcNow
        };

        var locations = new List<Location> { location };
        _mockLocationService.Setup(s => s.GetAllLocationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(locations);

        // Act
        var result = await _controller.GetAllLocations(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var locationDtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        var locationDto = locationDtos.First();
        
        Assert.Equal(25.12345678m, locationDto.Latitude);
        Assert.Equal(-80.98765432m, locationDto.Longitude);
    }
}