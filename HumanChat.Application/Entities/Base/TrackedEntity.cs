using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Entities.Base;

/// <summary>
///     Extends <see cref="EntityBase{TKey}"/> with auditing/tracking fields
/// </summary>
/// <typeparam name="TKey">The struct type used for the entity's primary key</typeparam>
public abstract record TrackedEntity<TKey> : EntityBase<TKey>, ITrackedEntity where TKey : struct
{
    /// <summary>
    ///     Reference to user who created this record
    /// </summary>
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? CreatedBy { get; set; }

    /// <summary>
    ///     Reference to user who last modified this record
    /// </summary>
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? UpdatedBy { get; set; }

    /// <summary>
    ///     Date/time (UTC) when this record was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Foreign key to <see cref="CreatedBy"/> (user who created the record)
    /// </summary>
    public int? CreatedById { get; set; }

    /// <summary>
    ///     Date/time (UTC) when this record was last modified
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    ///     Foreign key to <see cref="UpdatedBy"/> (user who last modified the record)
    /// </summary>
    public int? UpdatedById { get; set; }
}