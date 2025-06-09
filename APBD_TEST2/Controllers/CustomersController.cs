using APBD_TEST2.Models.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> AddCustomerWithTickets([FromBody] CustomerPurchasePOSTDTO dto)
        {
            try
            {
                await _customerService.AddCustomerTicketPurchaseAsync(dto);
                return Created("", null);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
    }
}