using System.Security.Claims;
using HumanChat.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HumanChat.Application.Common.Providers;

/// <summary>
///     Implementation of <see cref="ICurrentUserProvider" />
/// </summary>
/// <param name="httpContextAccessor">Http context accessor <see cref="IHttpContextAccessor"/></param>
public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    /// <inheritdoc />
    public (string? firstName, string? lastName, string? email) GetCurrentUser()
    {
        var claims = httpContextAccessor.HttpContext?.User.Claims.ToList() ?? [];
        
        var firstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        var lastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        return (firstName, lastName, email);
    }
}
