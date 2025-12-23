namespace Commerce.Domain.Entities;

public class Product
{
    public Guid Id { get; }
    public string Sku { get; }
    public string Name { get; }
    public decimal PriceAmount { get; }
    public string Currency { get; }

    public Product(Guid id, string sku, string name, decimal priceAmount, string currency)
    {
        if (priceAmount < 0) throw new ArgumentOutOfRangeException(nameof(priceAmount));
        Id = id;
        Sku = sku;
        Name = name;
        PriceAmount = priceAmount;
        Currency = currency;
    }
}
