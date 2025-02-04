namespace HumanChat.Application.Common.Interfaces;

/// <summary>
///     Provides current user data
/// </summary>
/// <remarks>
///     Data provided, first name, last name, email
/// </remarks>
public interface ICurrentUserProvider
{
    /// <summary>
    ///     Gets the current user identification
    /// </summary>
    /// <returns>Current user data</returns>
    (string? firstName, string? lastName, string? email) GetCurrentUser();
}
