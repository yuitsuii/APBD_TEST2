using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_TEST2.Models;

[Table("Ticket_Concert")]
public class TicketConcert
{
    [Key]
    public int TicketConcertId { get; set; }
    
    [Required]
    public int TicketId { get; set; }
    
    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; }
    
    [Required]
    public int ConcertId { get; set; }
    
    [ForeignKey("ConcertId")]
    public Concert Concert { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public ICollection<PurchasedTicket> PurchasedTickets { get; set; }
}