using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public class AuditDbContext : DbContext
{
    public AuditDbContext(DbContextOptions<AuditDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTrailLog> AuditTrailLogs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AuditTrailLogEntityTypeConfig().Configure(modelBuilder.Entity<AuditTrailLog>());
    }
}
