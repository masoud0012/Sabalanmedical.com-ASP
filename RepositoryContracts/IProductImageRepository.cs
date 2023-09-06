using System;
using Entities;
namespace RepositoryContracts
{
    public interface IProductImageRepository
    {
        Task<ProductImg>? AddProductImage(ProductImg request);
        Task<List<ProductImg>>? GetAllProductImages();
        Task<ProductImg>? GetProductImageByImageID(Guid ImageId);
        Task<List<ProductImg>>? GetProductImagesByProductID(Guid productId);
        Task<ProductImg>? UpdateProductImage(ProductImg updateRequest);
        Task<bool> DeleteProductImage(Guid id);
    }
}
