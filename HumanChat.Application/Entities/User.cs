using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities;

/// <summary>
///     Represents a user in the system (by ID, first/last name, email).
///     Inherits from EntityBase<int> because it uses an int PK.
/// </summary>
[Index(nameof(Email), IsUnique = true)]
public sealed record User : EntityBase<int>
{
    /// <summary>
    ///     First name
    /// </summary>
    [MaxLength(100)]
    [Required]
    public required string FirstName { get; set; } = null!;

    /// <summary>
    ///     Last name
    /// </summary>
    [MaxLength(200)]
    [Required]
    public required string LastName { get; set; } = null!;

    /// <summary>
    ///     Email address (unique index). Optional if no real email is provided.
    /// </summary>
    [MaxLength(200)]
    public string? Email { get; set; }
}