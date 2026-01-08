namespace Commerce.Domain.Entities;
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
public sealed class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime CreatedAtUtc { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;
    private Order() { }

    public Order(Guid id, Guid customerId, DateTime createdAtUtc)
    {
        Id = id;
        CustomerId = customerId;
        CreatedAtUtc = createdAtUtc;
    }

    public static Order Create(Guid customerId)
        => new(Guid.NewGuid(), customerId, DateTime.UtcNow);
    public void AddItem(Guid productId, int quanitity, decimal unitPrice)
    {
        _items.Add(new OrderItem(
            productId,
            quanitity,
            unitPrice
        ));
    }
}