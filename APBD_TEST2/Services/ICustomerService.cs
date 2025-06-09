using APBD_TEST2.Models.DTOs;

namespace APBD_TEST2.Services;

public interface ICustomerService
{
    Task<CustomerGETPurchasesDTO?> GetCustomerPurchasesAsync(int customerId);
    Task AddCustomerTicketPurchaseAsync(CustomerPurchasePOSTDTO dto);
}