using api.Attributes;
using Domain.DTOs.Landing;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/reports/{fishingReportId}/[controller]")]
[RequireAccountJwt]
public class LandingsController : BaseController
{
    private readonly ILandingService _landingService;

    public LandingsController(ILandingService landingService)
    {
        _landingService = landingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLandings(Guid fishingReportId, [FromQuery] int? limit, [FromQuery] string? next, CancellationToken cancellationToken = default)
    {
        var validationResult = ValidateLimit(limit);
        if (validationResult != null)
        {
            return validationResult;
        }

        var accountId = AccountId;
        
        if (limit.HasValue || !string.IsNullOrEmpty(next))
        {
            var paginatedLandings = await _landingService.GetLandingsByFishingReportIdPaginatedAsync(fishingReportId, accountId, limit ?? 25, next, cancellationToken);
            return Ok(paginatedLandings);
        }
        else
        {
            var landings = await _landingService.GetLandingsByFishingReportIdAsync(fishingReportId, accountId, cancellationToken);
            return Ok(landings);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLandingById(Guid fishingReportId, Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
        var landing = await _landingService.GetLandingByIdAsync(id, fishingReportId, accountId, cancellationToken);
        
        if (landing == null)
        {
            return NotFound();
        }

        return Ok(landing);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLanding(Guid fishingReportId, [FromBody] CreateLandingDto createLandingDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountId = AccountId;
        
        try
        {
            var landing = await _landingService.CreateLandingAsync(createLandingDto, fishingReportId, accountId, cancellationToken);
            
            return CreatedAtAction(nameof(GetLandingById), new { fishingReportId, id = landing.Id }, landing);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLanding(Guid fishingReportId, Guid id, [FromBody] UpdateLandingDto updateLandingDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountId = AccountId;
        var landing = await _landingService.UpdateLandingAsync(id, updateLandingDto, fishingReportId, accountId, cancellationToken);
        
        if (landing == null)
        {
            return NotFound();
        }

        return Ok(landing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLanding(Guid fishingReportId, Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
        var result = await _landingService.DeleteLandingAsync(id, fishingReportId, accountId, cancellationToken);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}