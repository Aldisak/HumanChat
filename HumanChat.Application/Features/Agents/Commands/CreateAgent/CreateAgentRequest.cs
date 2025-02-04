using HumanChat.Application.Entities.Enums;

namespace HumanChat.Application.Features.Agents.Commands.CreateAgent;

/// <summary>
/// Payload for creating a new Agent in an Organization.
/// </summary>
public sealed record CreateAgentRequest
{
    /// <summary>
    ///     Display or real name of the agent.
    /// </summary>
    public required string Name { get; set; } = null!;

    /// <summary>
    ///     Email address of the agent.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    ///     Status of the agent, e.g. "Online", "Offline", etc.
    ///     If omitted, defaults to "Offline".
    /// </summary>
    public AgentStatus? Status { get; set; }
}