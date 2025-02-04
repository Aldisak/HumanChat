using System.ComponentModel.DataAnnotations;
using HumanChat.Application.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities;

/// <summary>
///     Represents a top-level tenant entity (billing entity)
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public sealed record Tenant : TrackedEntity<long>
{
    /// <summary>
    ///     Name of the tenant
    /// </summary>
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; } = null!;

    /// <summary>
    ///     Description or notes about the tenant
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    ///     Collection of organizations that belong to this tenant
    /// </summary>
    public ICollection<Organization> Organizations { get; set; } = [];
}