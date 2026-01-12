using Commerce.Application.Products.Results;
using Commerce.Application.Interfaces.In;
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
    private readonly IProductImageStorage _imageStorage;

    public ProductService(IProductRepository repo, IProductImageUriBuilder imageUriBuilder, IProductImageStorage imageStorage)
    {
        _repo = repo;
        _imageUriBuilder = imageUriBuilder;
        _imageStorage = imageStorage;
    }

    public async Task<PagedQueryResult<ProductResult>> GetProductsAsync(GetProductsQuery query, CancellationToken ct)
    {
        var (products, totalCount) = await _repo.GetPagedAsync(query.SearchTerm, query.CategorySlug, query.Page, query.PageSize, ct);

        var results = products.Select(p =>
        {
            var primaryImage = p.GetPrimaryImage();
            var imageUri = _imageUriBuilder.BuildUri(primaryImage?.BlobName, 3600);
            return Map(p, imageUri);
        }).ToList();
        
        return new PagedQueryResult<ProductResult>(results, totalCount);
    }


    public async Task<ProductResult?> GetProductByIdAsync(Guid productId, CancellationToken ct)
    {
        var product = await _repo.GetProductByIdAsync(productId, ct);
        var primaryImage = product?.GetPrimaryImage();
        return product is null ? null : Map(product, _imageUriBuilder.BuildUri(primaryImage?.BlobName, 3600));
    }

    private static ProductResult Map(Product p, string? imageUri)
        => new(p.Id, p.Name, p.Sku, p.PriceAmount, imageUri);

    public async Task<ProductResult?> GetProductBySkuAsync(string sku, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(sku)) return null; 
        var product = await _repo.GetProductBySkuAsync(sku, ct);
        return product is null ? null : Map(product, _imageUriBuilder.BuildUri(product.Id.ToString(), 3600));
    }

    public async Task<AddProductResult> AddProductAsync(CreateProductCommand command, CancellationToken ct)
    {
        try
        {
            var categoryId = await _repo.GetCategoryIdBySlugAsync(command.CategorySlug, ct);
            if (categoryId is null)
                throw new InvalidOperationException("Category not found.");

            var product = Product.Create(
                sku: command.Sku,
                name: command.Name,
                categoryId: categoryId.Value,
                priceAmount: command.Price);

            await _repo.CreateAsync(product, ct);
            return new AddProductResult(true, product.Id, null);
        }
        catch (Exception ex)
        {
            return new AddProductResult(false, Guid.Empty, $"An error occurred while adding the product: {ex.Message}");
        }
    }

    public async Task<AddImageResult> AddImageAsync(AddProductImageCommand command, CancellationToken ct)
    {
        var product = await _repo.GetProductByIdAsync(command.ProductId, ct);
        if (product is null)
            return new AddImageResult(false, null, "Product not found.");

        var imageId = Guid.NewGuid();
        var extension = Path.GetExtension(command.FileName);
        var blobName = $"products/{command.ProductId}/images/{imageId}{extension}";

        if (command.Content.CanSeek)
            command.Content.Position = 0;

        BlobUploadResult upload;
        // ***Remember to failure test***
        try
        {   
            upload = await _imageStorage.UploadAsync(
                blobName,
                command.Content,
                command.ContentType
            );

            await _repo.AddImageAsync(    
                productId: command.ProductId,
                blobName: blobName,
                imageId: imageId,
                makePrimary: command.IsPrimary,
                ct
            );

            return new AddImageResult(
                Success: true,
                ImageId: imageId,
                UrlOrError: upload.Url
            );

        }
        catch (Exception ex)
        {
            try { await _imageStorage.DeleteIfExistsAsync(blobName); } 
            
            catch { return new AddImageResult(false, null, $"Failed to upload image: {ex.Message}");}

            return new AddImageResult(false, null, $"Failed to upload image: {ex.Message}");
        }
    }

}
