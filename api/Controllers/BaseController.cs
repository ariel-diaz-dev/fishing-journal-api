using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

/// <summary>
/// Common base controller providing shared functionality for all API controllers.
/// </summary>
public abstract class CommonBaseController : ControllerBase
{
    /// <summary>
    /// Maximum allowed limit for pagination requests
    /// </summary>
    protected const int MaxLimit = 100;

    /// <summary>
    /// Validates the limit parameter for pagination requests
    /// </summary>
    /// <param name="limit">The limit value to validate</param>
    /// <returns>BadRequest result if limit exceeds maximum, null if valid</returns>
    protected IActionResult? ValidateLimit(int? limit)
    {
        if (limit.HasValue && limit.Value > MaxLimit)
        {
            return BadRequest($"Limit cannot exceed {MaxLimit}");
        }
        return null;
    }
}

/// <summary>
/// Base controller for controllers that require account-based authorization.
/// Provides easy access to the authenticated account ID and common validation methods.
/// </summary>
public abstract class BaseController : CommonBaseController
{
    /// <summary>
    /// Gets the AccountId from HttpContext.Items that was set by the RequireAccountJwt attribute.
    /// This property should only be used in controllers decorated with [RequireAccountJwt].
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when AccountId is not found in HttpContext.Items</exception>
    protected Guid AccountId
    {
        get
        {
            if (HttpContext.Items.TryGetValue("AccountId", out var accountIdObj) && accountIdObj is Guid accountId)
            {
                return accountId;
            }

            throw new InvalidOperationException("AccountId not found in HttpContext. Ensure the controller is decorated with [RequireAccountJwt] attribute.");
        }
    }
}