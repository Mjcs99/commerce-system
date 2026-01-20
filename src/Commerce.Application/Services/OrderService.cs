namespace Commerce.Application.Services;

using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Commerce.Application.Orders.Commands;
using Commerce.Domain.Entities;
using Commerce.Application.Exceptions;
using System.Text.Json;
using Commerce.Contracts.IntegrationContracts.Orders;
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOutbox _outbox;
    private readonly IUnitOfWork _unitOfWork;
    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IOutbox outbox,
        IUnitOfWork unitOfWork
        )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _outbox = outbox;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateOrderAsync(PlaceOrderRequest request, Guid customerId, CancellationToken ct)
    {
        var order = Order.Create(customerId);
        foreach (var orderItem in request.Items) {
            var product = await _productRepository.GetProductByIdAsync(orderItem.ProductId, ct) ?? throw new NotFoundException($"Product with ID: {orderItem.ProductId} not found");
            if (orderItem.Quantity <= 0) throw new ValidationException("Quantity must be greater than 0.");
            order.AddItem(product.Id, orderItem.Quantity, product.PriceAmount);
        }
        
        _orderRepository.AddOrder(order);

        var evt = new OrderPlacedEvent(
            OrderId: order.Id,
            CustomerId: order.CustomerId,
            Items: [.. order.Items.Select(i =>
                new OrderPlacedItem(i.ProductId, i.Quantity, i.UnitPrice))]
        );
        
        var payload = JsonSerializer.Serialize(evt);
        _outbox.Enqueue("OrderPlaced", payload);    
     
        await _unitOfWork.SaveChangesAsync(ct);    
        
        return order.Id;
    }
}