using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace WebCommerce.Web.Web.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Qty { get; set; }

    public uint Version { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

public class ProductEntityTypeConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id).HasName("products_pk");
        builder.ToTable("products", tb =>
            tb.HasComment("Store the product list for sale."));

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .HasComment("Store product ID as primary key.");

        builder.Property(e => e.Version)
            .IsRowVersion();

        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Price).HasColumnName("price");
        builder.Property(e => e.Qty).HasColumnName("qty");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("created_at")
            .HasComment("Store the created date & time of the data.");

        builder.Property(e => e.CreatedBy)
            .HasDefaultValueSql("'system'::text")
            .HasColumnName("created_by")
            .HasComment("Store the name of who created the data.");

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy)
            .HasDefaultValueSql("'system'::text")
            .HasColumnName("updated_by");

        // Soft delete filter.
        builder.HasQueryFilter(e => e.DeletedAt == null);
    }
}