using Entities;
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

        public ProductImageResponse? AddProductImage(ProductImageAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductImg productImage = request.ToProductImage();
            _sabalanDbContext.ProductImgs.Add(productImage);
            _sabalanDbContext.SaveChanges();
            return productImage.ToProductImgResponse();
        }

        public bool DeleteProductImage(Guid? imageID)
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
            return true;
        }

        public List<ProductImageResponse>? GetAllProductImages()
        {
            return _sabalanDbContext.ProductImgs.Select(t => t.ToProductImgResponse()).ToList();
        }

        public ProductImageResponse? GetProductImageByImageID(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }
            ProductImg? productImg = _sabalanDbContext.ProductImgs.FirstOrDefault(t => t.ImageID == imageID);
            if (productImg == null)
            {
                throw new ArgumentException("No image was found!");
            }
            return productImg.ToProductImgResponse();
        }

        public List<ProductImageResponse>? GetProductImagesByProductID(Guid? productId)
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

        public ProductImageResponse? UpdateProductImage(ProductImageUpdateRequest? updateRequest)
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
            _sabalanDbContext.SaveChanges();
            return response.ToProductImgResponse();
        }
    }

}

