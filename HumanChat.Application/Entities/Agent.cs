using System.ComponentModel.DataAnnotations;
using HumanChat.Application.Entities.Base;
using HumanChat.Application.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities;

/// <summary>
///     Represents an agent (support representative) within an organization
/// </summary>
[Index(nameof(Email), IsUnique = true)]
public sealed record Agent : TrackedEntity<long>
{
    /// <summary>
    ///     Foreign key referencing the organization that this agent belongs to
    /// </summary>
    public long OrganizationId { get; set; }

    /// <summary>
    ///     The agent's display or real name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; } = null!;

    /// <summary>
    ///     The agent's email address
    /// </summary>
    [MaxLength(200)]
    [Required]
    public required string Email { get; set; } = null!;

    /// <summary>
    ///     The agent's current status <see cref="AgentStatus"/>
    /// </summary>
    public AgentStatus Status { get; set; } = AgentStatus.Offline;

    /// <summary>
    ///     Navigation reference to the parent Organization
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    ///     Collection of Chats this agent is participating in
    /// </summary>
    public ICollection<Chat> Chats { get; set; } = [];
}