using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductsDTO;
using Services.Helpers;
using System;

namespace Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly SabalanDbContext _sabalanDbContext;
        //constractor
        public ProductImageService(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }

        public async Task<ProductImageResponse>? AddProductImage(ProductImageAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductImg productImage = request.ToProductImage();
            _sabalanDbContext.ProductImgs.Add(productImage);
            await _sabalanDbContext.SaveChangesAsync();
            return productImage.ToProductImgResponse();
        }

        public async Task<bool> DeleteProductImage(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }
            ProductImg? response = _sabalanDbContext.ProductImgs.FirstOrDefault(t => t.ImageID == imageID);
            if (response is null)
            {
                return false;
            }
            _sabalanDbContext.ProductImgs.Remove(response);
            await _sabalanDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductImageResponse>>? GetAllProductImages()
        {
            return _sabalanDbContext.sp_GetAllProductImages().Select(t => t.ToProductImgResponse()).ToList();
        }

        public async Task<ProductImageResponse>? GetProductImageByImageID(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }
            ProductImg? productImg =await _sabalanDbContext.ProductImgs.FirstOrDefaultAsync(t => t.ImageID == imageID);
            if (productImg == null)
            {
                throw new ArgumentException("No image was found!");
            }
            return productImg.ToProductImgResponse();
        }

        public async Task<List<ProductImageResponse>>? GetProductImagesByProductID(Guid? productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            List<ProductImg>? response = _sabalanDbContext.ProductImgs.Where(t => t.ProductID == productId).ToList();
            if (response==null)
            {
                return null;
            }
            return response.Select(t => t.ToProductImgResponse()).ToList();
        }

        public async Task<ProductImageResponse>? UpdateProductImage(ProductImageUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ProductImg? response = _sabalanDbContext.ProductImgs.FirstOrDefault(t => t.ImageID == updateRequest.ImageID);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            response.ImageID = updateRequest.ImageID;
            response.ProductID = updateRequest.ProductID;
            response.ImageUrl = updateRequest.ImageUrl;
           await _sabalanDbContext.SaveChangesAsync();
            return response.ToProductImgResponse();
        }
    }

}

