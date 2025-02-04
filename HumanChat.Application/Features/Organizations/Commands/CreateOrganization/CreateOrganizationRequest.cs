namespace HumanChat.Application.Features.Organizations.Commands.CreateOrganization;

/// <summary>
/// Payload for creating a new Organization under a specific Tenant.
/// </summary>
public sealed record CreateOrganizationRequest
{
    /// <summary>
    ///     Name of the organization.
    /// </summary>
    public required string Name { get; set; } = null!;

    /// <summary>
    ///     Description about the organization.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Webhook URL for chat events.
    /// </summary>
    public string? WebhookUrl { get; set; }
}