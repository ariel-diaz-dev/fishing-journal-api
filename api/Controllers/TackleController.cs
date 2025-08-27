using api.Attributes;
using Domain.DTOs.Tackle;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequireAccountJwt]
public class TackleController : ControllerBase
{
    private readonly ITackleService _tackleService;

    public TackleController(ITackleService tackleService)
    {
        _tackleService = tackleService;
    }

    private Guid GetAccountIdFromHttpContext()
    {
        return (Guid)HttpContext.Items["AccountId"]!;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTackle(CancellationToken cancellationToken = default)
    {
        var accountId = GetAccountIdFromHttpContext();
        var tackle = await _tackleService.GetAllTackleByAccountAsync(accountId, cancellationToken);
        
        var tackleDtos = tackle.Select(t => new TackleDto
        {
            Id = t.Id,
            AccountId = t.AccountId,
            Type = t.Type,
            Name = t.Name,
            Description = t.Description,
            CreatedDate = t.CreatedDate,
            DeletedDate = t.DeletedDate,
            UpdatedDate = t.UpdatedDate
        });

        return Ok(tackleDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTackleById(Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = GetAccountIdFromHttpContext();
        var tackle = await _tackleService.GetTackleByIdAsync(id, accountId, cancellationToken);
        
        if (tackle == null)
        {
            return NotFound();
        }

        var tackleDto = new TackleDto
        {
            Id = tackle.Id,
            AccountId = tackle.AccountId,
            Type = tackle.Type,
            Name = tackle.Name,
            Description = tackle.Description,
            CreatedDate = tackle.CreatedDate,
            DeletedDate = tackle.DeletedDate,
            UpdatedDate = tackle.UpdatedDate
        };

        return Ok(tackleDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTackle([FromBody] CreateTackleDto createTackleDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountId = GetAccountIdFromHttpContext();
        
        try
        {
            var tackle = await _tackleService.CreateTackleAsync(accountId, createTackleDto, cancellationToken);
            
            var tackleDto = new TackleDto
            {
                Id = tackle.Id,
                AccountId = tackle.AccountId,
                Type = tackle.Type,
                Name = tackle.Name,
                Description = tackle.Description,
                CreatedDate = tackle.CreatedDate,
                DeletedDate = tackle.DeletedDate,
                UpdatedDate = tackle.UpdatedDate
            };

            return CreatedAtAction(nameof(GetTackleById), new { id = tackle.Id }, tackleDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTackle(Guid id, [FromBody] UpdateTackleDto updateTackleDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountId = GetAccountIdFromHttpContext();
        var tackle = await _tackleService.UpdateTackleAsync(id, accountId, updateTackleDto, cancellationToken);
        
        if (tackle == null)
        {
            return NotFound();
        }

        var tackleDto = new TackleDto
        {
            Id = tackle.Id,
            AccountId = tackle.AccountId,
            Type = tackle.Type,
            Name = tackle.Name,
            Description = tackle.Description,
            CreatedDate = tackle.CreatedDate,
            DeletedDate = tackle.DeletedDate,
            UpdatedDate = tackle.UpdatedDate
        };

        return Ok(tackleDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTackle(Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = GetAccountIdFromHttpContext();
        var result = await _tackleService.DeleteTackleAsync(id, accountId, cancellationToken);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}