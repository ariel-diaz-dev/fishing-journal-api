using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace api.Attributes;

public class RequireAccountAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // First check if user is authenticated (JWT is valid)
        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Extract and validate accountId claim
        var accountIdClaim = context.HttpContext.User.FindFirst("accountId")?.Value;
        if (accountIdClaim == null || !Guid.TryParse(accountIdClaim, out var accountId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Store accountId in HttpContext for easy access
        context.HttpContext.Items["AccountId"] = accountId;
    }
}