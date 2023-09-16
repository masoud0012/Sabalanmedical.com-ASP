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
        Task<ProductImageResponse>? AddProductImage(ProductImageAddRequest? request);
        Task<List<ProductImageResponse>>? GetAllProductImages();
        Task<ProductImageResponse>? GetProductImageByImageID(Guid? id);
        Task<List<ProductImageResponse>>? GetProductImagesByProductID(Guid? productId);
        Task<ProductImageResponse>? UpdateProductImage(ProductImageUpdateRequest? updateRequest);
        Task<bool> DeleteProductImage(Guid? id);
    }
}
