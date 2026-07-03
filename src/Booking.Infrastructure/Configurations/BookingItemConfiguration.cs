using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class BookingItemConfiguration : IEntityTypeConfiguration<BookingItem>
{
    public void Configure(EntityTypeBuilder<BookingItem> builder)
    {
        builder.ToTable("booking_items");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("booking_item_id");

        builder.Property(i => i.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(i => i.EventZoneId)
            .HasColumnName("event_zone_id")
            .IsRequired();

        builder.Property(i => i.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(i => i.Booking)
            .WithMany(r => r.BookingItems)
            .HasForeignKey(i => i.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.EventZone)
            .WithMany(z => z.BookingItems)
            .HasForeignKey(i => i.EventZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
