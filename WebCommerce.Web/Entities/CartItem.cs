using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public partial class CartItem
{
    public Guid UserAccountId { get; set; }

    public Guid ProductId { get; set; }

    public int Qty { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual UserAccount UserAccount { get; set; } = null!;
}

public class CartItemEntityTypeConfig : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(e => new { e.UserAccountId, e.ProductId }).HasName("cart_items_pk");

        builder.ToTable("cart_items");

        builder.Property(e => e.UserAccountId).HasColumnName("user_account_id");
        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.Qty).HasColumnName("qty");

        builder.HasOne(d => d.Product).WithMany(p => p.CartItems)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("cart_items__products_fk");

        builder.HasOne(d => d.UserAccount).WithMany(p => p.CartItems)
            .HasForeignKey(d => d.UserAccountId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("cart_items__user_accounts");

        // Soft delete filter.
        builder.HasQueryFilter(e => e.Product.DeletedAt == null);
    }
}