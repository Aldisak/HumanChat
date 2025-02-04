using System.ComponentModel.DataAnnotations;
using HumanChat.Application.Entities.Base;
using HumanChat.Application.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities;

/// <summary>
///     Represents a single message in a chat
/// </summary>
[Index(nameof(ChatId))]
public sealed record Message : TrackedEntity<long>
{
    /// <summary>
    ///     Foreign key referencing the chat that this message belongs to
    /// </summary>
    [Required]
    public required long ChatId { get; set; }

    /// <summary>
    ///     ID of the sender (e.g. AgentId, or a "CustomerId" - 
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string SenderId { get; set; } = null!;

    /// <summary>
    ///     The type of sender, e.g. "Agent" or "Customer" (Enum)
    /// </summary>
    public SenderType SenderType { get; set; } = SenderType.Customer;

    /// <summary>
    ///     The message content or body text
    /// </summary>
    [Required]
    [MaxLength(2000)]
    public required string Content { get; set; } = null!;

    /// <summary>
    ///     Timestamp (UTC) of when the message was created/sent
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Navigation reference to the parent Chat
    /// </summary>
    public Chat? Chat { get; set; }
}