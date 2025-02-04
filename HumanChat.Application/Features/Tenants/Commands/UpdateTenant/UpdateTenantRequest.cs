namespace HumanChat.Application.Features.Tenants.Commands.UpdateTenant;

/// <summary>
///     Payload for updating (patching) an existing Tenant.
/// </summary>
public sealed record UpdateTenantRequest
{
    /// <summary>
    ///     Optional new name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Optional new description
    /// </summary>
    public string? Description { get; set; }
}