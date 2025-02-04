namespace HumanChat.Application.Features.Tenants.Commands.CreateTenant;

/// <summary>
///     Request payload for creating a new Tenant.
/// </summary>
public sealed record CreateTenantRequest
{
    /// <summary>
    ///     Name of the tenant (required).
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Optional description.
    /// </summary>
    public string? Description { get; set; }
}