using HumanChat.Application.Entities.Enums;

namespace HumanChat.Application.Features.Agents.Commands.UpdateAgent;

/// <summary>
/// Payload for updating (patching) an existing Agent.
/// </summary>
public sealed record UpdateAgentRequest
{
    /// <summary>
    ///     Optional new name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Optional new email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Optional new status
    /// </summary>
    public AgentStatus? Status { get; set; }
}