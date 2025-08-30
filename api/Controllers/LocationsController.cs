using api.Attributes;
using Domain.DTOs.Common;
using Domain.DTOs.Location;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequireAccountJwt]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<LocationDto>>> GetAllLocations([FromQuery] int? limit, [FromQuery] string? next, CancellationToken cancellationToken)
    {
        var paginatedLocations = await _locationService.GetAllLocationsPaginatedAsync(limit ?? 25, next, cancellationToken);
        
        var locationDtos = paginatedLocations.Data.Select(l => new LocationDto
        {
            Id = l.Id,
            Name = l.Name,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Description = l.Description,
            Order = l.Order,
            CreatedDate = l.CreatedDate
        });

        var response = new PaginatedResponse<LocationDto>
        {
            Data = locationDtos,
            NextCursor = paginatedLocations.NextCursor,
            HasMore = paginatedLocations.HasMore,
            Count = paginatedLocations.Count,
            Limit = paginatedLocations.Limit
        };

        return Ok(response);
    }
}