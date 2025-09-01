using api.Attributes;
using api.Controllers;
using Domain.DTOs.Common;
using Domain.DTOs.FishSpecies;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/species")]
[RequireAccountJwt]
public class FishSpeciesController : CommonBaseController
{
    private readonly IFishSpeciesService _fishSpeciesService;

    public FishSpeciesController(IFishSpeciesService fishSpeciesService)
    {
        _fishSpeciesService = fishSpeciesService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<FishSpeciesDto>>> GetAllFishSpecies([FromQuery] int? limit, [FromQuery] string? next, CancellationToken cancellationToken)
    {
        if (limit.HasValue && limit.Value > MaxLimit)
        {
            return BadRequest($"Limit cannot exceed {MaxLimit}");
        }

        var paginatedFishSpecies = await _fishSpeciesService.GetAllFishSpeciesPaginatedAsync(limit ?? 25, next, cancellationToken);
        
        var fishSpeciesDtos = paginatedFishSpecies.Data.Select(fs => new FishSpeciesDto
        {
            Id = fs.Id,
            Order = fs.Order,
            Name = fs.Name,
            ScientificName = fs.ScientificName,
            Description = fs.Description,
            CreatedDate = fs.CreatedDate
        });

        var response = new PaginatedResponse<FishSpeciesDto>
        {
            Data = fishSpeciesDtos,
            NextCursor = paginatedFishSpecies.NextCursor,
            HasMore = paginatedFishSpecies.HasMore,
            Count = paginatedFishSpecies.Count,
            Limit = paginatedFishSpecies.Limit
        };

        return Ok(response);
    }
}