namespace HumanChat.Application.Entities.Enums;

/// <summary>
///     Represents the current status of an agent
/// </summary>
public enum AgentStatus
{
    /// <summary>
    ///    The agent is offline
    /// </summary>
    Offline,

    /// <summary>
    ///     The agent is online and available
    /// </summary>
    Online,

    /// <summary>
    ///     The agent is away from their desk
    /// </summary>
    Away,

    /// <summary>
    ///     The agent is currently busy
    /// </summary>
    Busy
}