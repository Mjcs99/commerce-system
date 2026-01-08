namespace Commerce.Application.Services;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Commerce.Application.Orders.Commands;
using Commerce.Application.Orders.Results;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public Task<CreateOrderResult> CreateOrderAsync(CreateOrderCommand command)
    {
        throw new NotImplementedException();
    }
}