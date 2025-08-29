using api.Attributes;
using Domain.DTOs.Common;
using Domain.DTOs.FishingReport;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequireAccountJwt]
public class FishingReportsController : BaseController
{
    private readonly IFishingReportService _fishingReportService;

    public FishingReportsController(IFishingReportService fishingReportService)
    {
        _fishingReportService = fishingReportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFishingReports([FromQuery] int? limit, [FromQuery] string? next, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
        
        if (limit.HasValue || !string.IsNullOrEmpty(next))
        {
            var paginatedReports = await _fishingReportService.GetFishingReportsByAccountIdPaginatedAsync(accountId, limit ?? 25, next, cancellationToken);
            return Ok(paginatedReports);
        }
        else
        {
            var fishingReports = await _fishingReportService.GetFishingReportsByAccountIdAsync(accountId, cancellationToken);
            return Ok(fishingReports);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFishingReportById(Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
        var fishingReport = await _fishingReportService.GetFishingReportByIdAsync(id, accountId, cancellationToken);
        
        if (fishingReport == null)
        {
            return NotFound();
        }

        return Ok(fishingReport);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFishingReport([FromBody] CreateFishingReportDto createFishingReportDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountId = AccountId;
        
        try
        {
            var fishingReport = await _fishingReportService.CreateFishingReportAsync(createFishingReportDto, accountId, cancellationToken);
            
            return CreatedAtAction(nameof(GetFishingReportById), new { id = fishingReport.Id }, fishingReport);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFishingReport(Guid id, [FromBody] UpdateFishingReportDto updateFishingReportDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountId = AccountId;
        var fishingReport = await _fishingReportService.UpdateFishingReportAsync(id, updateFishingReportDto, accountId, cancellationToken);
        
        if (fishingReport == null)
        {
            return NotFound();
        }

        return Ok(fishingReport);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFishingReport(Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
        var result = await _fishingReportService.DeleteFishingReportAsync(id, accountId, cancellationToken);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}