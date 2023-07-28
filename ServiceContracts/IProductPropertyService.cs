using ServiceContracts.DTO.ProductPropertyDTO;
using System;


namespace ServiceContracts
{
    public interface IProductPropertyService
    {
        ProductPropertyResponse? AddProductProperty(ProductPropertyAddRequest request);
        List<ProductPropertyResponse>? GetAllProductProperty();
        ProductPropertyResponse? GetProductPropertyByPropertyID(Guid? propertyId);
        List<ProductPropertyResponse>? GetProductPropertiesByProductID(Guid? productId);
        ProductPropertyResponse? UpdateProductProperty(ProductPropertyUpdateRequest? updateRequest);
        bool DeleteProductProperty(Guid? id);
    }
}
