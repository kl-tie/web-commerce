using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public partial class UserAccount
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}

public class UserAccountEntityTypeConfig : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(e => e.Id).HasName("user_accounts_pk");

        builder.ToTable("user_accounts");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        builder.Property(e => e.FullName).HasColumnName("full_name");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("created_at");
        builder.Property(e => e.CreatedBy)
            .HasDefaultValueSql("'system'::text")
            .HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy)
            .HasDefaultValueSql("'system'::text")
            .HasColumnName("updated_by");
    }
}