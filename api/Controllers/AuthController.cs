using Domain.DTOs.Auth;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IAccountService _accountService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IJwtService jwtService, IAccountService accountService, IOptions<JwtSettings> jwtSettings)
    {
        _jwtService = jwtService;
        _accountService = accountService;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GenerateToken([FromBody] AuthRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var accountExists = await _accountService.AccountExistsAsync(request.AccountId, cancellationToken);
        if (!accountExists)
        {
            return BadRequest("Account not found");
        }

        var token = _jwtService.GenerateToken(request.AccountId);
        var response = new AuthResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            AccountId = request.AccountId
        };

        return Ok(response);
    }
}