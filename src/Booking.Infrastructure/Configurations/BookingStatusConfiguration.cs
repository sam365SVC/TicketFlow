using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class BookingStatusConfiguration : IEntityTypeConfiguration<BookingStatus>
{
    public void Configure(EntityTypeBuilder<BookingStatus> builder)
    {
        builder.ToTable("booking_statuses");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("status_id");

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .HasMaxLength(255);
    }
}
