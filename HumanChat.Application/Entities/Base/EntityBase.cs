using Microsoft.EntityFrameworkCore;

/// <summary>
///     Abstract base record for all entities, containing a primary key of type <typeparamref name="TKey"/>
/// </summary>
/// <typeparam name="TKey">The struct type used for the entity's primary key</typeparam>
[PrimaryKey(nameof(Id))]
public abstract record EntityBase<TKey> where TKey : struct
{
    /// <summary>
    ///     Entity identification (primary key)
    /// </summary>
    public TKey Id { get; set; }
}