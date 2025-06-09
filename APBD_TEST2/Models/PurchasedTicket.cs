using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBD_TEST2.Models;

[Table("Purchased_Ticket")]
[PrimaryKey(nameof(TicketConcertId),nameof(CustomerId))]
public class PurchasedTicket
{
    [Required]
    public int TicketConcertId { get; set; }
    
    [ForeignKey("TicketConcertId")]
    public TicketConcert TicketConcert { get; set; }
    
    [Required]
    public int CustomerId { get; set; }
    
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    
    [Required]
    public DateTime PurchaseDate { get; set; }
    
}