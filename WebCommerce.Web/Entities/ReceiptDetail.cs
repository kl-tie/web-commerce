using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public partial class ReceiptDetail
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }

    public Guid ProductId { get; set; }

    public decimal Price { get; set; }

    public int Qty { get; set; }

    public virtual Receipt Receipt { get; set; } = null!;
}

public class ReceiptDetailEntityTypeConfig : IEntityTypeConfiguration<ReceiptDetail>
{
    public void Configure(EntityTypeBuilder<ReceiptDetail> builder)
    {
        builder.HasKey(e => e.Id).HasName("receipt_details_pk");

        builder.ToTable("receipt_details");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        builder.Property(e => e.Price).HasColumnName("price");
        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.Qty).HasColumnName("qty");
        builder.Property(e => e.ReceiptId).HasColumnName("receipt_id");

        builder.HasOne(d => d.Receipt).WithMany(p => p.ReceiptDetails)
            .HasForeignKey(d => d.ReceiptId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("receipt_details__receipts_fk");
    }
}