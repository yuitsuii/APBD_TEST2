using APBD_TEST2.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TEST2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}/purchases")]
        public async Task<IActionResult> GetCustomerPurchases(int id)
        {
            var result = await _customerService.GetCustomerPurchasesAsync(id);
            if (result == null)
            {
                return NotFound($"Customer with id {id} not found.");
            }
            return Ok(result);
        }

    }
}