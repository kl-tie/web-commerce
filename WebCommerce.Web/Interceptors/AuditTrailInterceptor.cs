using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebCommerce.Web.Web.Entities;
using System.Text.Json;

namespace WebCommerce.Web.Web.Interceptors;

public class AuditTrailInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        if (eventData.Context is not null)
        {
            AddAuditLog(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        if (eventData.Context is not null)
        {
            AddAuditLog(eventData.Context);
        }

        return result;
    }

    private void AddAuditLog(DbContext context)
    {
        var options = new DbContextOptionsBuilder<AuditDbContext>();
        options.UseNpgsql(context.Database.GetConnectionString());
        options.UseSnakeCaseNamingConvention();

        using var auditContext = new AuditDbContext(options.Options);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            var actionType = entry.State switch
            {
                EntityState.Added => "Create",
                EntityState.Modified => "Update",
                _ => "Deleted"
            };

            auditContext.AuditTrailLogs.Add(new AuditTrailLog
            {
                Id = Guid.NewGuid(),
                ActionType = actionType,
                Data = JsonSerializer.Serialize(new 
                {
                    Name = entry.Metadata.DisplayName(),
                    Detail = entry.Properties.Aggregate($"Modifying {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ")
                })
            });
        }

        auditContext.SaveChanges();
    }
}
