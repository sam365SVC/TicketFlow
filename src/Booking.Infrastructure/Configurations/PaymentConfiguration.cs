using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("payment_id");

        builder.Property(p => p.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(p => p.PaymentStatusId)
            .HasColumnName("payment_status_id")
            .IsRequired();

        builder.Property(p => p.Amount)
            .HasColumnName("amount")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasColumnName("payment_method")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.ExternalTransactionId)
            .HasColumnName("external_transaction_id")
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne(p => p.Booking)
            .WithMany(r => r.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.PaymentStatus)
            .WithMany()
            .HasForeignKey(p => p.PaymentStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
