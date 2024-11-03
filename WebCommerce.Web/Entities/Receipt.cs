using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public partial class Receipt
{
    public Guid Id { get; set; }

    public Guid? UserAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();

    public virtual UserAccount? UserAccount { get; set; }
}

public class ReceiptEntityTypeConfig : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.HasKey(e => e.Id).HasName("receipts_pk");
        builder.ToTable("receipts");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        builder.Property(e => e.UserAccountId).HasColumnName("user_account_id");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("created_at");
        builder.Property(e => e.CreatedBy)
            .HasDefaultValueSql("'system'::text")
            .HasColumnName("created_by");

        builder.HasOne(d => d.UserAccount).WithMany(p => p.Receipts)
            .HasForeignKey(d => d.UserAccountId)
            .HasConstraintName("receipts__account_id_fk");
    }
}