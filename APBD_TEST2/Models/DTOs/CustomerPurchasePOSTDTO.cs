using System.Text.Json.Serialization;

namespace APBD_TEST2.Models.DTOs;

public class CustomerPurchasePOSTDTO
{
    [JsonPropertyName("customer")]
    public CustomerDTO CustomerDTO { get; set; }
    
    [JsonPropertyName("purchases")]
    public List<PurchaseRequestDTO> Purchases { get; set; }
}

public class CustomerDTO
{
    [JsonPropertyName("id")]
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}

public class PurchaseRequestDTO
{
    public int SeatNumber { get; set; }
    public string ConcertName { get; set; }
    public decimal Price { get; set; }
}