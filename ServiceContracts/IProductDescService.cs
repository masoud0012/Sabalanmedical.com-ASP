using ServiceContracts.DTO.ProductDescriptionDTO;
using System;


namespace ServiceContracts
{
    public interface IProductDescService
    {
        ProductDescResponse? AddProductDesc(ProductDescAddRequest? request);
        List<ProductDescResponse>? GetAllProductDesc();
        ProductDescResponse? GetProductDescByDescID(Guid? descID);
        List<ProductDescResponse>? GetProductDescByProductID(Guid? productID);
        ProductDescResponse? UpdateProductDesc(ProductDescUpdateRequest? updateRequest);
        bool DeleteProductDesc(Guid? id);
    }
}
