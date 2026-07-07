using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class EventZoneConfiguration : IEntityTypeConfiguration<EventZone>
{
    public void Configure(EntityTypeBuilder<EventZone> builder)
    {
        builder.ToTable("event_zones");

        builder.HasKey(z => z.Id);
        builder.Property(z => z.Id).HasColumnName("event_zone_id");

        builder.Property(z => z.EventId)
            .HasColumnName("event_id")
            .IsRequired();

        builder.Property(z => z.TicketType)
            .HasColumnName("ticket_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(z => z.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(z => z.Capacity)
            .HasColumnName("capacity")
            .IsRequired();

        builder.Property(z => z.TicketsSold)
            .HasColumnName("tickets_sold")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(z => z.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(z => z.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne(z => z.Event)
            .WithMany(e => e.EventZones)
            .HasForeignKey(z => z.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(z => new { z.EventId, z.TicketType })
            .IsUnique()
            .HasDatabaseName("uq_event_zones_event_ticket_type");
    }
}
