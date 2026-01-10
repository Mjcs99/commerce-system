using Microsoft.AspNetCore.Mvc;
using Commerce.Application.Interfaces.In;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Commerce.Domain.Entities;
using Commerce.Application.Orders.Commands;
namespace Commerce.Api.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;
    
    public OrderController(IOrderService orderService, ICustomerService customerService)
    {
        _orderService = orderService;
        _customerService = customerService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceOrderAsync(
        [FromQuery] Guid productId,
        [FromQuery] int quantity
    )
    {
        var externalUserId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (externalUserId == null || email == null) return Unauthorized();
        // Change get or create customer ?
        var customer = await _customerService.GetOrCreateCustomerAsync(externalUserId, email, firstName: null, lastName: null);
        if (customer == null) return Unauthorized();
        var res = await _orderService.CreateOrderAsync(new CreateOrderCommand(customer.Id, productId, quantity));
        return Ok(res);
    }
}