using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class TicketStatusConfiguration : IEntityTypeConfiguration<TicketStatus>
{
    public void Configure(EntityTypeBuilder<TicketStatus> builder)
    {
        builder.ToTable("ticket_statuses");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("ticket_status_id");

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .HasMaxLength(255);
    }
}
