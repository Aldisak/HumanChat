namespace HumanChat.Application.Features.Organizations.Commands.UpdateOrganization;

/// <summary>
/// Payload for updating an existing Organization.
/// </summary>
public sealed record UpdateOrganizationRequest
{
    /// <summary>
    ///     Name of the organization.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Description about the organization.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Webhook URL for chat events.
    /// </summary>
    public string? WebhookUrl { get; set; }
}