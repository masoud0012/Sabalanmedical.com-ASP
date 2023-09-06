using System;
using Entities;

namespace RepositoryContracts
{
    public interface IProductPropertiesRepository
    {
        Task<ProductProperty>? AddProductProperty(ProductProperty request);
        Task<List<ProductProperty>>? GetAllProductProperty();
        Task<ProductProperty>? GetProductPropertyByPropertyID(Guid propertyId);
        Task<List<ProductProperty>>? GetProductPropertiesByProductID(Guid productId);
        Task<ProductProperty>? UpdateProductProperty(ProductProperty updateRequest);
        Task<bool> DeleteProductProperty(ProductProperty productProperty);
    }
}