using HumanChat.Application.Entities.Base;
using HumanChat.Application.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities;

/// <summary>
///     Represents a chat session within an organization
/// </summary>
[Index(nameof(OrganizationId))]
public sealed record Chat : TrackedEntity<long>
{
    /// <summary>
    ///     Foreign key referencing the organization that owns this chat
    /// </summary>
    public required long OrganizationId { get; set; }

    /// <summary>
    ///     Current status of the chat, e.g. "active", "closed"
    /// </summary>
    public ChatStatus Status { get; set; } = ChatStatus.Active;

    /// <summary>
    ///     Name of the customer (if provided)
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    ///     Email of the customer (if provided)
    /// </summary>
    public string? CustomerEmail { get; set; }

    /// <summary>
    ///     Timestamp (UTC) when the chat started
    /// </summary>
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Timestamp (UTC) when the chat ended (if closed)
    /// </summary>
    public DateTime? EndedAt { get; set; }
    
    /// <summary>
    ///     Navigation reference to the parent Organization
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    ///     Collection of Messages that belong to this chat
    /// </summary>
    public ICollection<Message> Messages { get; set; } = [];

    /// <summary>
    ///     Collection of Agents that are participating in this chat
    /// </summary>
    public ICollection<Agent> Agents { get; set; } = [];
}