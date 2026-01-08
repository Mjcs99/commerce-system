namespace Commerce.Domain.Entities;
public class InventoryItem
{
    public Guid ProductId { get; set; }
    public int QuantityAvailable { get; set; }

}