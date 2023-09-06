using System;
using Entities;
namespace RepositoryContracts
{
    public interface IProductDescriptionRepository
    {
        Task<ProductDesc>? AddProductDesc(ProductDesc request);
        Task<List<ProductDesc>>? GetAllProductDesc();
        Task<ProductDesc>? GetProductDescByDescID(Guid descID);
        Task<List<ProductDesc>>? GetProductDescByProductID(Guid productID);
        Task<ProductDesc>? UpdateProductDesc(ProductDesc updateRequest);
        Task<bool> DeleteProductDesc(Guid id);
    }
}
