namespace CTMS.Data;

using Microsoft.EntityFrameworkCore;
using CTMS.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}