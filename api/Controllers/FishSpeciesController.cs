using Domain.DTOs.FishSpecies;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/species")]
public class FishSpeciesController : ControllerBase
{
    private readonly IFishSpeciesService _fishSpeciesService;

    public FishSpeciesController(IFishSpeciesService fishSpeciesService)
    {
        _fishSpeciesService = fishSpeciesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FishSpeciesDto>>> GetAllFishSpecies(CancellationToken cancellationToken)
    {
        var fishSpecies = await _fishSpeciesService.GetAllFishSpeciesAsync(cancellationToken);
        
        var fishSpeciesDtos = fishSpecies.Select(fs => new FishSpeciesDto
        {
            Id = fs.Id,
            Order = fs.Order,
            Name = fs.Name,
            ScientificName = fs.ScientificName,
            Description = fs.Description,
            CreatedDate = fs.CreatedDate
        });

        return Ok(fishSpeciesDtos);
    }
}