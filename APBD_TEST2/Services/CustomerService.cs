using APBD_TEST2.Context;
using APBD_TEST2.Models;
using APBD_TEST2.Models.DTOs;
using Microsoft.EntityFrameworkCore;


namespace APBD_TEST2.Services;

public class CustomerService : ICustomerService
{
    private readonly ConcertContext _context;

    public CustomerService(ConcertContext context)
    {
        _context = context;
    }


    public async Task<CustomerGETPurchasesDTO?> GetCustomerPurchasesAsync(int customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.CustomerId == customerId).Select(c => new CustomerGETPurchasesDTO
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Purchases = c.PurchasedTickets.Select(pt => new PurchaseDTO
                {
                    Date = pt.PurchaseDate,
                    Price = pt.TicketConcert.Price,
                    Ticket = new TicketDTO
                    {
                        SerialNumber = pt.TicketConcert.Ticket.SerialNumber,
                        SeatNumber = pt.TicketConcert.Ticket.SeatNumber
                    },
                    Concert = new ConcertDTO
                    {
                        Name = pt.TicketConcert.Concert.Name,
                        Date = pt.TicketConcert.Concert.Date
                    }

                }).ToList()
            }).FirstOrDefaultAsync();
        return customer;
    }

    public async Task AddCustomerTicketPurchaseAsync(CustomerPurchasePOSTDTO dto)
    {
        var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == dto.CustomerDTO.CustomerId);
        
        if (customer == null)
        {
            customer = new Customer
            {
                FirstName = dto.CustomerDTO.FirstName,
                LastName = dto.CustomerDTO.LastName,
                PhoneNumber = dto.CustomerDTO.PhoneNumber
            };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync(); 
        }
        
        var purchasesGroupedByConcert = dto.Purchases
            .GroupBy(p => p.ConcertName);

        foreach (var concertGroup in purchasesGroupedByConcert)
        {
            string concertName = concertGroup.Key;
            var concert = await _context.Concerts
                .FirstOrDefaultAsync(c => c.Name == concertName);

            if (concert == null)
                throw new ArgumentException($"Concert '{concertName}' does not exist.");

            int existingTickets = await _context.PurchasedTickets
                .Include(pt => pt.TicketConcert)
                .CountAsync(pt => pt.TicketConcert.ConcertId == concert.ConcertId && pt.CustomerId == customer.CustomerId);

            int requestedTickets = concertGroup.Count();

            if (existingTickets + requestedTickets > 5)
                throw new ArgumentException($"Customer cannot buy more than 5 tickets for concert: '{concertName}'.");
            
            var maximumTickets = await  _context.TicketConcerts.Where(concert => concert.ConcertId == concert.ConcertId)
                .Select(concert => concert.Concert.AvailableTickets).CountAsync();
            
            if (existingTickets + requestedTickets > maximumTickets)
                throw new ArgumentException($"Ticket limit exceeded for concert: '{concertName}'.");
            
            foreach (var purchase in concertGroup)
            {
                var ticket = new Ticket
                { 
                    SeatNumber = purchase.SeatNumber,
                    SerialNumber = "TEMP"
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                ticket.SerialNumber = "TEMP";
                await _context.SaveChangesAsync();

                var ticketConcert = new TicketConcert
                {
                    TicketId = ticket.TicketId,
                    ConcertId = concert.ConcertId,
                    Price = purchase.Price
                };

                await _context.TicketConcerts.AddAsync(ticketConcert);
                await _context.SaveChangesAsync();

                var purchased = new PurchasedTicket
                {
                    CustomerId = customer.CustomerId,
                    TicketConcertId = ticketConcert.TicketConcertId,
                    PurchaseDate = DateTime.Now
                };

                await _context.PurchasedTickets.AddAsync(purchased);
            }
        }
        await _context.SaveChangesAsync();
    }
}