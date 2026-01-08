using System.Dynamic;

namespace Commerce.Domain.Entities;
public sealed class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPriceAmount { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid productId, int quantity, decimal unitPriceAmount)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPriceAmount = unitPriceAmount;
    }

    public static OrderItem Create(Guid productId, int quantity, decimal unitPriceAmount)
        => new(productId, quantity, unitPriceAmount);
}