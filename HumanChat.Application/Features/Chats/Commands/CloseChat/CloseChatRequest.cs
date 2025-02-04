namespace HumanChat.Application.Features.Chats.Commands.CloseChat;

/// <summary>
/// Payload for closing (ending) an active Chat.
/// Optionally includes who closed it, or reason, etc.
/// </summary>
public sealed class CloseChatRequest
{
    /// <summary>
    ///     Optional field to record who closed the chat
    /// </summary>
    public string? ClosedBy { get; set; }

    /// <summary>
    ///     Optional reason for closing the chat
    /// </summary>
    public string? Reason { get; set; }
}