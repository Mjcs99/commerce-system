namespace Commerce.Application.Services;

using System.ComponentModel.DataAnnotations;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Commerce.Application.Orders.Commands;
using Commerce.Application.Orders.Results;
using Commerce.Domain.Entities;
using Commerce.Application.Exceptions;
using System.Text.Json;
using Commerce.Contracts.Orders;


public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOutbox _outbox;
    private readonly IUnitOfWork _unitOfWork;
    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IOutbox outbox,
        IUnitOfWork unitOfWork
        )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _outbox = outbox;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateOrderResult> CreateOrderAsync(CreateOrderCommand command)
    {
        if (command.Quantity <= 0)
            throw new ValidationException("Quantity must be greater than 0.");

        var product = await _productRepository.GetProductByIdAsync(command.ProductId)
            ?? throw new NotFoundException($"Product with ID: {command.ProductId} not found");

        var order = Order.Create(command.CustomerId);
        order.AddItem(command.ProductId, command.Quantity, product.PriceAmount);

        _orderRepository.AddOrder(order);

        var evt = new OrderPlacedEvent(
            OrderId: order.Id,
            CustomerId: order.CustomerId,
            Items: [.. order.Items.Select(i =>
                new OrderPlacedItem(i.ProductId, i.Quantity, i.UnitPrice))]
        );

        _outbox.Enqueue("OrderPlaced", JsonSerializer.Serialize(evt));

        await _unitOfWork.SaveChangesAsync();

        return new CreateOrderResult(true, order.Id);
    }
}