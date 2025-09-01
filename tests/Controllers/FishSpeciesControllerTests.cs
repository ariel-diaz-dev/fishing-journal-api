using Microsoft.AspNetCore.Mvc;
using Moq;
using Domain.Interfaces;
using Domain.Models;
using Domain.DTOs.Common;
using Domain.DTOs.FishSpecies;
using Api.Controllers;

namespace Tests.Controllers;

public class FishSpeciesControllerTests
{
    private readonly Mock<IFishSpeciesService> _mockFishSpeciesService;
    private readonly FishSpeciesController _controller;

    public FishSpeciesControllerTests()
    {
        _mockFishSpeciesService = new Mock<IFishSpeciesService>();
        _controller = new FishSpeciesController(_mockFishSpeciesService.Object);
    }

    [Fact]
    public async Task GetAllFishSpecies_ReturnsOkWithFishSpeciesList()
    {
        // Arrange
        var fishSpecies1 = new FishSpecies
        {
            Id = 1,
            Order = 1,
            Name = "Common Snook",
            ScientificName = "Centropomus undecimalis",
            Description = "Highly prized gamefish with distinctive black lateral line.",
            CreatedDate = DateTime.UtcNow
        };

        var fishSpecies2 = new FishSpecies
        {
            Id = 2,
            Order = 2,
            Name = "Tarpon",
            ScientificName = "Megalops atlanticus",
            Description = "The 'Silver King' - massive gamefish known for spectacular jumps.",
            CreatedDate = DateTime.UtcNow
        };

        var fishSpecies = new List<FishSpecies> { fishSpecies1, fishSpecies2 };

        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpecies,
            NextCursor = null,
            HasMore = false,
            Count = fishSpecies.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var paginatedResponseResult = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesArray = paginatedResponseResult.Data.ToArray();
        
        Assert.Equal(2, fishSpeciesArray.Length);
        Assert.Equal(1, fishSpeciesArray[0].Id);
        Assert.Equal("Common Snook", fishSpeciesArray[0].Name);
        Assert.Equal("Centropomus undecimalis", fishSpeciesArray[0].ScientificName);
        Assert.Equal(1, fishSpeciesArray[0].Order);
        Assert.Equal("Highly prized gamefish with distinctive black lateral line.", fishSpeciesArray[0].Description);
        
        Assert.Equal(2, fishSpeciesArray[1].Id);
        Assert.Equal("Tarpon", fishSpeciesArray[1].Name);
        Assert.Equal("Megalops atlanticus", fishSpeciesArray[1].ScientificName);
        Assert.Equal(2, fishSpeciesArray[1].Order);
    }

    [Fact]
    public async Task GetAllFishSpecies_EmptyList_ReturnsOkWithEmptyList()
    {
        // Arrange
        var fishSpecies = new List<FishSpecies>();
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpecies,
            NextCursor = null,
            HasMore = false,
            Count = fishSpecies.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        Assert.Empty(actualResponse.Data);
    }

    [Fact]
    public async Task GetAllFishSpecies_ServiceThrowsException_ThrowsException()
    {
        // Arrange
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            _controller.GetAllFishSpecies(null, null, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllFishSpecies_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var fishSpecies = new List<FishSpecies>();
        var cancellationToken = CancellationToken.None;
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpecies,
            NextCursor = null,
            HasMore = false,
            Count = fishSpecies.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, cancellationToken))
            .ReturnsAsync(paginatedResponse);

        // Act
        await _controller.GetAllFishSpecies(null, null, cancellationToken);

        // Assert
        _mockFishSpeciesService.Verify(s => s.GetAllFishSpeciesPaginatedAsync(25, null, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAllFishSpecies_MapsAllFishSpeciesProperties()
    {
        // Arrange
        var createdDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var fishSpecies = new FishSpecies
        {
            Id = 99,
            Order = 15,
            Name = "Test Fish",
            ScientificName = "Testicus fishicus",
            Description = "A test fish species with special characters: @#$%^&*()",
            CreatedDate = createdDate
        };

        var fishSpeciesList = new List<FishSpecies> { fishSpecies };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesDto = actualResponse.Data.First();
        
        Assert.Equal(fishSpecies.Id, fishSpeciesDto.Id);
        Assert.Equal(fishSpecies.Order, fishSpeciesDto.Order);
        Assert.Equal(fishSpecies.Name, fishSpeciesDto.Name);
        Assert.Equal(fishSpecies.ScientificName, fishSpeciesDto.ScientificName);
        Assert.Equal(fishSpecies.Description, fishSpeciesDto.Description);
        Assert.Equal(fishSpecies.CreatedDate, fishSpeciesDto.CreatedDate);
    }

    [Fact]
    public async Task GetAllFishSpecies_HandlesNullScientificName()
    {
        // Arrange
        var fishSpecies = new FishSpecies
        {
            Id = 1,
            Order = 1,
            Name = "Fish Without Scientific Name",
            ScientificName = null,
            Description = "A fish without a scientific name",
            CreatedDate = DateTime.UtcNow
        };

        var fishSpeciesList = new List<FishSpecies> { fishSpecies };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesDto = actualResponse.Data.First();
        
        Assert.Null(fishSpeciesDto.ScientificName);
    }

    [Fact]
    public async Task GetAllFishSpecies_HandlesNullDescription()
    {
        // Arrange
        var fishSpecies = new FishSpecies
        {
            Id = 1,
            Order = 1,
            Name = "Fish Without Description",
            ScientificName = "Fishicus nodescriptius",
            Description = null,
            CreatedDate = DateTime.UtcNow
        };

        var fishSpeciesList = new List<FishSpecies> { fishSpecies };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesDto = actualResponse.Data.First();
        
        Assert.Null(fishSpeciesDto.Description);
    }

    [Fact]
    public async Task GetAllFishSpecies_ReturnsFishSpeciesInCorrectOrder()
    {
        // Arrange
        var fishSpecies1 = new FishSpecies { Id = 1, Name = "First Fish", Order = 1, CreatedDate = DateTime.UtcNow };
        var fishSpecies2 = new FishSpecies { Id = 2, Name = "Second Fish", Order = 2, CreatedDate = DateTime.UtcNow };
        var fishSpecies3 = new FishSpecies { Id = 3, Name = "Third Fish", Order = 3, CreatedDate = DateTime.UtcNow };

        // Return them in the order that the repository would (ordered by Order field)
        var fishSpeciesList = new List<FishSpecies> { fishSpecies1, fishSpecies2, fishSpecies3 };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesArray = actualResponse.Data.ToArray();
        
        Assert.Equal(3, fishSpeciesArray.Length);
        Assert.Equal(1, fishSpeciesArray[0].Order);
        Assert.Equal(2, fishSpeciesArray[1].Order);
        Assert.Equal(3, fishSpeciesArray[2].Order);
        Assert.Equal("First Fish", fishSpeciesArray[0].Name);
        Assert.Equal("Second Fish", fishSpeciesArray[1].Name);
        Assert.Equal("Third Fish", fishSpeciesArray[2].Name);
    }

    [Fact]
    public async Task GetAllFishSpecies_HandlesLongDescription()
    {
        // Arrange
        var longDescription = new string('A', 1000); // Maximum length description
        var fishSpecies = new FishSpecies
        {
            Id = 1,
            Order = 1,
            Name = "Fish With Long Description",
            ScientificName = "Longicus descripticus",
            Description = longDescription,
            CreatedDate = DateTime.UtcNow
        };

        var fishSpeciesList = new List<FishSpecies> { fishSpecies };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesDto = actualResponse.Data.First();
        
        Assert.Equal(longDescription, fishSpeciesDto.Description);
        Assert.Equal(1000, fishSpeciesDto.Description?.Length);
    }

    [Fact]
    public async Task GetAllFishSpecies_HandlesSpecialCharactersInNames()
    {
        // Arrange
        var fishSpecies = new FishSpecies
        {
            Id = 1,
            Order = 1,
            Name = "Fish (Common Name)",
            ScientificName = "Species name-with-hyphens",
            Description = "Description with \"quotes\" and 'apostrophes'",
            CreatedDate = DateTime.UtcNow
        };

        var fishSpeciesList = new List<FishSpecies> { fishSpecies };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesDto = actualResponse.Data.First();
        
        Assert.Equal("Fish (Common Name)", fishSpeciesDto.Name);
        Assert.Equal("Species name-with-hyphens", fishSpeciesDto.ScientificName);
        Assert.Equal("Description with \"quotes\" and 'apostrophes'", fishSpeciesDto.Description);
    }

    [Fact]
    public async Task GetAllFishSpecies_WithMultipleFishSpecies_MaintainsDataIntegrity()
    {
        // Arrange
        var createdDate1 = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var createdDate2 = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc);
        
        var fishSpecies1 = new FishSpecies
        {
            Id = 1,
            Order = 5,
            Name = "Redfish",
            ScientificName = "Sciaenops ocellatus",
            Description = "Copper-bronze fish with black spots",
            CreatedDate = createdDate1
        };

        var fishSpecies2 = new FishSpecies
        {
            Id = 2,
            Order = 10,
            Name = "Snook",
            ScientificName = "Centropomus undecimalis",
            Description = "Prized gamefish with black lateral line",
            CreatedDate = createdDate2
        };

        var fishSpeciesList = new List<FishSpecies> { fishSpecies1, fishSpecies2 };
        var paginatedResponse = new PaginatedResponse<FishSpecies>
        {
            Data = fishSpeciesList,
            NextCursor = null,
            HasMore = false,
            Count = fishSpeciesList.Count,
            Limit = 25
        };
        _mockFishSpeciesService.Setup(s => s.GetAllFishSpeciesPaginatedAsync(25, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAllFishSpecies(null, null, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PaginatedResponse<FishSpeciesDto>>(okResult.Value);
        var fishSpeciesArray = actualResponse.Data.ToArray();
        
        // Verify first fish species
        Assert.Equal(1, fishSpeciesArray[0].Id);
        Assert.Equal(5, fishSpeciesArray[0].Order);
        Assert.Equal("Redfish", fishSpeciesArray[0].Name);
        Assert.Equal("Sciaenops ocellatus", fishSpeciesArray[0].ScientificName);
        Assert.Equal("Copper-bronze fish with black spots", fishSpeciesArray[0].Description);
        Assert.Equal(createdDate1, fishSpeciesArray[0].CreatedDate);
        
        // Verify second fish species
        Assert.Equal(2, fishSpeciesArray[1].Id);
        Assert.Equal(10, fishSpeciesArray[1].Order);
        Assert.Equal("Snook", fishSpeciesArray[1].Name);
        Assert.Equal("Centropomus undecimalis", fishSpeciesArray[1].ScientificName);
        Assert.Equal("Prized gamefish with black lateral line", fishSpeciesArray[1].Description);
        Assert.Equal(createdDate2, fishSpeciesArray[1].CreatedDate);
    }
}