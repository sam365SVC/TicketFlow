using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("ticket_id");

        builder.Property(t => t.BookingItemId)
            .HasColumnName("booking_item_id")
            .IsRequired();

        builder.Property(t => t.Status)
            .HasColumnName("ticket_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.TicketCode)
            .HasColumnName("ticket_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.UsedAt)
            .HasColumnName("used_at");

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne(t => t.BookingItem)
            .WithMany(i => i.Tickets)
            .HasForeignKey(t => t.BookingItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.TicketCode)
            .IsUnique()
            .HasDatabaseName("uq_tickets_ticket_code");
    }
}
