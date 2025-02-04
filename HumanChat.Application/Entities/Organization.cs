using System.ComponentModel.DataAnnotations;
using HumanChat.Application.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities;

/// <summary>
///     Represents a subdivision or unit under a Tenant
/// </summary>
[Index(nameof(TenantId), nameof(Name), IsUnique = true)]
public sealed record Organization : TrackedEntity<long>
{
    /// <summary>
    ///     Foreign key referencing the owning Tenant
    /// </summary>
    public required long TenantId { get; set; }

    /// <summary>
    ///     The name of this organization
    /// </summary>
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; } = null!;

    /// <summary>
    ///     Description or notes about the organization
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    ///     Webhook URL for receiving chat events
    /// </summary>
    [MaxLength(500)]
    public string? WebhookUrl { get; set; }

    /// <summary>
    ///     Navigation reference to the parent Tenant
    /// </summary>
    public Tenant? Tenant { get; set; }

    /// <summary>
    ///     Collection of Agents belonging to this organization
    /// </summary>
    public ICollection<Agent> Agents { get; set; } = [];

    /// <summary>
    ///     Collection of Chats belonging to this organization
    /// </summary>
    public ICollection<Chat> Chats { get; set; } = [];
}