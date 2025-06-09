using APBD_TEST2.Context;
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
}