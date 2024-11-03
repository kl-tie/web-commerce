using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebCommerce.Web.Web.Entities;

public partial class AuditTrailLog
{
    public Guid Id { get; set; }

    public string Data { get; set; } = null!;

    public string ActionType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}

public class AuditTrailLogEntityTypeConfig : IEntityTypeConfiguration<AuditTrailLog>
{
    public void Configure(EntityTypeBuilder<AuditTrailLog> builder)
    {
        builder.HasKey(e => e.Id).HasName("audit_trail_logs_pk");

        builder.ToTable("audit_trail_logs");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        builder.Property(e => e.Data)
            .HasColumnType("jsonb")
            .HasColumnName("data");
        builder.Property(e => e.ActionType)
            .HasColumnName("action_type");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("created_at");
    }
}