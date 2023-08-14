using ServiceContracts.DTO.ProductPropertyDTO;
using System;


namespace ServiceContracts
{
    public interface IProductPropertyService
    {
        Task<ProductPropertyResponse>? AddProductProperty(ProductPropertyAddRequest request);
        Task<List<ProductPropertyResponse>>? GetAllProductProperty();
        Task<ProductPropertyResponse>? GetProductPropertyByPropertyID(Guid? propertyId);
        Task<List<ProductPropertyResponse>>? GetProductPropertiesByProductID(Guid? productId);
        Task<ProductPropertyResponse>? UpdateProductProperty(ProductPropertyUpdateRequest? updateRequest);
        Task<bool> DeleteProductProperty(Guid? id);
    }
}
