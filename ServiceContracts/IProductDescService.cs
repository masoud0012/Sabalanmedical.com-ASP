using ServiceContracts.DTO.ProductDescriptionDTO;
using System;


namespace ServiceContracts
{
    public interface IProductDescService
    {
        Task<ProductDescResponse>? AddProductDesc(ProductDescAddRequest? request);
        Task<List<ProductDescResponse>>? GetAllProductDesc();
        Task<ProductDescResponse>? GetProductDescByDescID(Guid? descID);
        Task<List<ProductDescResponse>>? GetProductDescByProductID(Guid? productID);
        Task<ProductDescResponse>? UpdateProductDesc(ProductDescUpdateRequest? updateRequest);
        Task<bool> DeleteProductDesc(Guid? id);
    }
}
