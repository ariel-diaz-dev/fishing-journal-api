using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

/// <summary>
/// Base controller for controllers that require account-based authorization.
/// Provides easy access to the authenticated account ID.
/// </summary>
public abstract class BaseController : ControllerBase
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