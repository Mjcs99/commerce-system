using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commerce.Application.DTOs;

namespace Commerce.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
}
