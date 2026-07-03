using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class PaymentStatusConfiguration : IEntityTypeConfiguration<PaymentStatus>
{
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable("payment_statuses");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("payment_status_id");

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .HasMaxLength(255);
    }
}
