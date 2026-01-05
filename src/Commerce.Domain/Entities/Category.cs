namespace Commerce.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;

    private Category() { } 

    public Category(Guid id, string name, string slug)
    {
        Id = id;
        Name = name;
        Slug = slug;
    }
    public static Category Create(string name, string slug)
        => new(Guid.NewGuid(), name, slug);
}
    