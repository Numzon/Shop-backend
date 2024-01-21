using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Common;
using Shop.Infrastructure.Extensions;

namespace Shop.Infrastructure.Persistance.Interceptors;
public sealed class BaseAuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public BaseAuditableEntitySaveChangesInterceptor(ICurrentUserService currentUser, IDateTime dateTime)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateBaseAudiableEntityProperties(eventData.Context!);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)  
    {
        UpdateBaseAudiableEntityProperties(eventData.Context!);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateBaseAudiableEntityProperties(DbContext context)
    { 
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = _dateTime.Now;
                entry.Entity.CreatedBy = _currentUser.UserId;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModified = _dateTime.Now;
                entry.Entity.LastModifiedBy = _currentUser.UserId;
            }
        }
    }
}