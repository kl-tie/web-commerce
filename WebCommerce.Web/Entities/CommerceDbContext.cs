using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public partial class CommerceDbContext : DbContext
{
    public CommerceDbContext(DbContextOptions<CommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTrailLog> AuditTrailLogs { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Reference: https://learn.microsoft.com/en-us/ef/core/modeling/#applying-all-configurations-in-an-assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Product).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
