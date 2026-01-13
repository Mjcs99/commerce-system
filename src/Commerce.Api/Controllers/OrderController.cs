using Microsoft.AspNetCore.Mvc;
using Commerce.Application.Interfaces.In;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        [FromQuery] int quantity,
        CancellationToken ct)
    {
        var externalUserId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (externalUserId == null || email == null) return Unauthorized();
        var firstName = User.FindFirstValue(ClaimTypes.GivenName);
        var lastName = User.FindFirstValue(ClaimTypes.Surname);;
        var customer = await _customerService.GetOrCreateCustomerAsync(externalUserId, email, firstName, lastName, ct);
        if (customer == null) return Unauthorized();
        var res = await _orderService.CreateOrderAsync(new CreateOrderCommand(customer.Id, productId, quantity), ct);
        return Ok(res);
    }
}