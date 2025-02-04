namespace HumanChat.Application.Entities.Enums;

/// <summary>
///     Represents the type of sender for a message
/// </summary>
public enum SenderType
{
    /// <summary>
    ///     The sender is an agent
    /// </summary>
    Agent,

    /// <summary>
    ///     The sender is a customer
    /// </summary>
    Customer,

    /// <summary>
    ///     The sender is the system
    /// </summary>
    System
}