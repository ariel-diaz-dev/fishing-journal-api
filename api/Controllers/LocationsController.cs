using api.Attributes;
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
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetAllLocations(CancellationToken cancellationToken)
    {
        var locations = await _locationService.GetAllLocationsAsync(cancellationToken);
        
        var locationDtos = locations.Select(l => new LocationDto
        {
            Id = l.Id,
            Name = l.Name,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Description = l.Description,
            Order = l.Order,
            CreatedDate = l.CreatedDate
        });

        return Ok(locationDtos);
    }
}