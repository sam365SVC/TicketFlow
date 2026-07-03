using Booking.Domain;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TicketType> TicketTypes => Set<TicketType>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventZone> EventZones => Set<EventZone>();
    public DbSet<BookingStatus> BookingStatuses => Set<BookingStatus>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<BookingItem> BookingItems => Set<BookingItem>();
    public DbSet<TicketStatus> TicketStatuses => Set<TicketStatus>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
    }
}
