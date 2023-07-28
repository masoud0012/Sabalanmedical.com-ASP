using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IProductImageService
    {
        ProductImageResponse? AddProductImage(ProductImageAddRequest? request);
        List<ProductImageResponse>? GetAllProductImages();
        ProductImageResponse? GetProductImageByImageID(Guid? id);
        List<ProductImageResponse>? GetProductImagesByProductID(Guid? productId);
        ProductImageResponse? UpdateProductImage(ProductImageUpdateRequest? updateRequest);
        bool DeleteProductImage(Guid? id);
    }
}
