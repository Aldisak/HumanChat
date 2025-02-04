using HumanChat.Application.Entities;
using HumanChat.Application.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace HumanChat.Application.Infrastructure.Persistence;

/// <summary>
///     Database context for HumanChat database
/// </summary>
/// <param name="options"><see cref="DbContextOptions{TContext}" /> for <see cref="HumanChatDbContext" /></param>
/// <param name="trackingInterceptor">
///     Interceptor which sets info on creation and modification before saving
///     <see cref="TrackingInterceptor" />
/// </param>
public class HumanChatDbContext(DbContextOptions<HumanChatDbContext> options, TrackingInterceptor trackingInterceptor)
    : DbContext(options)
{
    /// <summary>
    ///     Users <see cref="User" />
    /// </summary>
    public virtual DbSet<User> Users => Set<User>();

    /// <summary>
    ///     Tenants <see cref="Tenant" />
    /// </summary>
    public virtual DbSet<Tenant> Tenants => Set<Tenant>();

    /// <summary>
    ///     Organizations <see cref="Organization" />
    /// </summary>
    public virtual DbSet<Organization> Organizations => Set<Organization>();

    /// <summary>
    ///     Agents <see cref="Agent" />
    /// </summary>
    public virtual DbSet<Agent> Agents => Set<Agent>();

    /// <summary>
    ///     Chats <see cref="Chat" />
    /// </summary>
    public virtual DbSet<Chat> Chats => Set<Chat>();

    /// <summary>
    ///     Messages <see cref="Message" />
    /// </summary>
    public virtual DbSet<Message> Messages => Set<Message>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumanChatDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(trackingInterceptor);
    }
}