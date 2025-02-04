using HumanChat.Application.Common.Interfaces;
using HumanChat.Application.Entities;
using HumanChat.Application.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HumanChat.Application.Infrastructure.Persistence.Interceptors;

/// <summary>
///     The interceptor user in <see cref="HumanChatDbContext" /> to add audit information to entities based on
///     <see cref="TrackedEntity{TKey}" />
/// </summary>
/// <param name="currentUserProvider">Provides current user identification <see cref="ICurrentUserProvider" /></param>
public class TrackingInterceptor(ICurrentUserProvider currentUserProvider) : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var changeTracker = eventData.Context?.ChangeTracker;
        if (changeTracker is null || !changeTracker.HasChanges()) return base.SavingChanges(eventData, result);

        SetAuditData(eventData.Context!);

        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                          InterceptionResult<int> result,
                                                                          CancellationToken cancellationToken = default)
    {
        var changeTracker = eventData.Context?.ChangeTracker;
        if (changeTracker is null || !changeTracker.HasChanges())
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        SetAuditData(eventData.Context!);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditData(DbContext dbContext)
    {
        var entries = dbContext.ChangeTracker.Entries();
        var utcNow = DateTime.UtcNow;
        var currentUser = GetUser(dbContext);

        foreach (var changedEntry in entries)
        {
            if (changedEntry.Entity is not ITrackedEntity entity)
                continue;

            switch (changedEntry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = entity.CreatedAt == default ? utcNow : entity.CreatedAt;
                    entity.CreatedBy = currentUser;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = utcNow;
                    entity.UpdatedBy = currentUser;
                    break;
            }
        }
    }

    private User? GetUser(DbContext dbContext)
    {
        var (firstName, lastName, email) = currentUserProvider.GetCurrentUser();

        if (string.IsNullOrWhiteSpace(email))
            return null;

        var user = dbContext.Set<User>().FirstOrDefault(u => u.Email == email);

        return UpdateOrCreateUser(dbContext, user, firstName, lastName, email);
    }

    private static User UpdateOrCreateUser(DbContext dbContext, User? user, string? firstName, string? lastName, 
                                           string? email)
    {
        if (user is null)
        {
            user = new User
            {
                FirstName = firstName ?? string.Empty,
                LastName  = lastName  ?? string.Empty,
                Email = email
            };

            dbContext.Add(user);
        }
        else
        {
            user.FirstName = firstName ?? string.Empty;
            user.LastName  = lastName  ?? string.Empty;
            user.Email = email ?? string.Empty;
            dbContext.Update(user);
        }

        return user;
    }
}