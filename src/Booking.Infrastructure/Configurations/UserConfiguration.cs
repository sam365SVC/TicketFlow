using Booking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("user_id");

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(u => u.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.FailedAttempts)
            .HasColumnName("failed_attempts")
            .HasDefaultValue((short)0)
            .IsRequired();

        builder.Property(u => u.Active)
            .HasColumnName("active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.RequiresPwdChange)
            .HasColumnName("requires_pwd_change")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.LockedUntil)
            .HasColumnName("locked_until");

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(u => u.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
