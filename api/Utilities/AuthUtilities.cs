namespace api.Utilities;

/// <summary>
/// Utility class for authentication and authorization operations
/// </summary>
public static class AuthUtilities
{
    /// <summary>
    /// Extracts the AccountId from HttpContext.Items that was set by the RequireAccountJwt attribute
    /// </summary>
    /// <param name="httpContext">The HTTP context containing the AccountId</param>
    /// <returns>The AccountId as a Guid</returns>
    /// <exception cref="InvalidOperationException">Thrown when AccountId is not found in HttpContext.Items</exception>
    public static Guid GetAccountIdFromHttpContext(HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue("AccountId", out var accountIdObj) && accountIdObj is Guid accountId)
        {
            return accountId;
        }

        throw new InvalidOperationException("AccountId not found in HttpContext. Ensure the controller is decorated with [RequireAccountJwt] attribute.");
    }
}