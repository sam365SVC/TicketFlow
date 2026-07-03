using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("event_id");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.Location)
            .HasColumnName("location")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.EventDateTime)
            .HasColumnName("event_datetime")
            .IsRequired();

        builder.Property(e => e.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);

        builder.Property(e => e.IsPublished)
            .HasColumnName("is_published")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
