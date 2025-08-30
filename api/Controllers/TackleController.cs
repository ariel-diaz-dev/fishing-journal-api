using api.Attributes;
using Domain.DTOs.Common;
using Domain.DTOs.Tackle;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequireAccountJwt]
public class TackleController : BaseController
{
    private readonly ITackleService _tackleService;

    public TackleController(ITackleService tackleService)
    {
        _tackleService = tackleService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllTackle([FromQuery] int? limit, [FromQuery] string? next, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
        var paginatedTackle = await _tackleService.GetAllTackleByAccountPaginatedAsync(accountId, limit ?? 25, next, cancellationToken);
        
        var tackleDtos = paginatedTackle.Data.Select(t => new TackleDto
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

        var response = new PaginatedResponse<TackleDto>
        {
            Data = tackleDtos,
            NextCursor = paginatedTackle.NextCursor,
            HasMore = paginatedTackle.HasMore,
            Count = paginatedTackle.Count,
            Limit = paginatedTackle.Limit
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTackleById(Guid id, CancellationToken cancellationToken = default)
    {
        var accountId = AccountId;
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

        var accountId = AccountId;
        
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

        var accountId = AccountId;
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
        var accountId = AccountId;
        var result = await _tackleService.DeleteTackleAsync(id, accountId, cancellationToken);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}