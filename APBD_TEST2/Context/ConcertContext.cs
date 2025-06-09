using APBD_TEST2.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_TEST2.Context;

public class ConcertContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<TicketConcert> TicketConcerts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

    protected ConcertContext() { }

    public ConcertContext(DbContextOptions<ConcertContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketConcert>().ToTable("Ticket_Concert");
        modelBuilder.Entity<PurchasedTicket>().ToTable("Purchased_Ticket");

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket { TicketId = 1, SerialNumber = "SN001", SeatNumber = 101 },
            new Ticket { TicketId = 2, SerialNumber = "SN002", SeatNumber = 102 }
        );

        modelBuilder.Entity<Concert>().HasData(
            new Concert { ConcertId = 1, Name = "Rock Fest", Date = new DateTime(2025, 7, 1), AvailableTickets = 100 },
            new Concert { ConcertId = 2, Name = "Jazz Night", Date = new DateTime(2025, 8, 15), AvailableTickets = 50 }
        );

        modelBuilder.Entity<TicketConcert>().HasData(
            new TicketConcert { TicketConcertId = 1, TicketId = 1, ConcertId = 1, Price = 99.99M },
            new TicketConcert { TicketConcertId = 2, TicketId = 2, ConcertId = 2, Price = 79.99M }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "555123456" },
            new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", PhoneNumber = "555987654" }
        );

        modelBuilder.Entity<PurchasedTicket>().HasData(
            new PurchasedTicket
            {
                TicketConcertId = 1,
                CustomerId = 1,
                PurchaseDate = new DateTime(2025, 6, 1)
            },
            new PurchasedTicket
            {
                TicketConcertId = 2,
                CustomerId = 2,
                PurchaseDate = new DateTime(2025, 6, 2)
            }
        );
    }
}