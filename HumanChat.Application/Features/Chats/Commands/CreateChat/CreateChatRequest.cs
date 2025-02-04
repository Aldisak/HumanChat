namespace HumanChat.Application.Features.Chats.Commands.CreateChat;

/// <summary>
/// Request payload for creating (starting) a new chat.
/// </summary>
public sealed class CreateChatRequest
{
    /// <summary>
    ///     Name of the customer (optional).
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    ///     Email of the customer (optional).
    /// </summary>
    public string? CustomerEmail { get; set; }
}