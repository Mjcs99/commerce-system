using Commerce.Application.Products.Results;
using Commerce.Application.Interfaces;
using Commerce.Domain.Entities;
using Commerce.Application.Products.Queries;
using Commerce.Application.Products.Commands;
using Commerce.Application.Common;
using Commerce.Application.Images;
using Commerce.Application.Interfaces.Out;

namespace Commerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IProductImageUriBuilder _imageUriBuilder;
    private readonly IBlobStorage _blobStorage;

    public ProductService(IProductRepository repo, IProductImageUriBuilder imageUriBuilder, IBlobStorage blobStorage)
    {
        _repo = repo;
        _imageUriBuilder = imageUriBuilder;
        _blobStorage = blobStorage;
    }

    public async Task<PagedQueryResult<ProductResult>> GetProductsAsync(GetProductsQuery query)
    {
        var (products, totalCount) = await _repo.GetPagedAsync(query.SearchTerm, query.Page, query.PageSize);
        var productResults = products.Select(p => Map(p, _imageUriBuilder.BuildUri(p.Id.ToString()))).ToList();
        return new PagedQueryResult<ProductResult>(productResults, totalCount);
    }

    public async Task<ProductResult?> GetProductByIdAsync(Guid productId)
    {
        var product = await _repo.GetProductByIdAsync(productId);
        return product is null ? null : Map(product, _imageUriBuilder.BuildUri(product.Id.ToString()));
    }

    private static ProductResult Map(Product p, string? imageUri)
        => new(p.Id, p.Name, p.Sku, p.PriceAmount, imageUri);

    public async Task<ProductResult?> GetProductBySkuAsync(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku)) return null; 
        var product = await _repo.GetProductBySkuAsync(sku);
        return product is null ? null : Map(product, _imageUriBuilder.BuildUri(product.Id.ToString()));
    }

    public async Task<Guid> AddProductAsync(CreateProductCommand command)
    {
        var product = new Product(Guid.NewGuid(), command.Name, command.Sku, command.Price);
        await _repo.CreateAsync(product);
        return product.Id;
    }

    public async Task<AddImageResult> AddImageAsync(AddProductImageCommand command)
    {
        var product = await _repo.GetProductByIdAsync(command.ProductId);
        if (product is null)
            return new AddImageResult(false, Guid.NewGuid(), "Product not found.");

        var imageId = Guid.NewGuid();
        var extension = Path.GetExtension(command.FileName);
        var blobName = $"products/{command.ProductId}/images/{imageId}{extension}";

        if (command.Content.CanSeek)
            command.Content.Position = 0;

        BlobUploadResult upload;
        try
        {
            upload = await _blobStorage.UploadAsync(
                blobName,
                command.Content,
                command.ContentType
            );
        }
        catch (Exception ex)
        {
            return new AddImageResult(false, Guid.NewGuid(), $"Upload failed: {ex.Message}");
        }

        return new AddImageResult(
            Success: true,
            ImageId: imageId,
            UrlOrError: upload.Url
        );
    }
}
