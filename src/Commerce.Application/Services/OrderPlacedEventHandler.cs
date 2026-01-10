using System.Text.Json;
using Commerce.Application.Exceptions;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Commerce.Contracts.IntegrationContracts.Orders;

namespace Commerce.Application.Services;

public class OrderPlacedEventHandler : IIntegrationEventHandler
{
    private readonly IOrderRepository _orderRepo;
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutbox _outbox;
    public OrderPlacedEventHandler(IOrderRepository orderRepo, IInventoryRepository inventoryRepo, IOutbox outbox, IUnitOfWork unitOfWork)
    {
        _orderRepo = orderRepo;
        _inventoryRepo = inventoryRepo;
        _unitOfWork = unitOfWork;
        _outbox = outbox;
    }

    public bool CanHandle(string type)
    {
        return true && type == "OrderPlaced";
    }

    public async Task HandleAsync(string type, string payload, CancellationToken ct)
    {
        var evtPlaced = JsonSerializer.Deserialize<OrderPlacedEvent>(
            payload,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (evtPlaced == null) return;
        
        var order = await _orderRepo.GetByIdAsync(evtPlaced.OrderId, ct) 
            ?? throw new NotFoundException("Order does not exist");
            
        order.UpdateStatus();

        IEnumerable<OrderPlacedItem> items = evtPlaced.Items;
     
        foreach (var item in items) await _inventoryRepo.ReserveAsync(item.ProductId, item.Qauntity, ct);
        
        var evtProcessed = new OrderProcessedEvent(
            OrderId: order.Id,
            CustomerId: order.CustomerId,
            Items: items.Select(oi => new OrderProcessedItem(
                ProductId: oi.ProductId, 
                Qauntity: oi.Qauntity, 
                UnitPrice: oi.UnitPrice
                ))
                .ToList()
        );

        _outbox.Enqueue("OrderProcessed", JsonSerializer.Serialize(evtProcessed));
        
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
