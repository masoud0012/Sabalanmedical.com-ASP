using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductsDTO;
using Services.Helpers;
using System;

namespace Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        //constractor
        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<ProductImageResponse>? AddProductImage(ProductImageAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductImg productImage = request.ToProductImage();
            await _productImageRepository.AddProductImage(request.ToProductImage());
            return productImage.ToProductImgResponse();
        }

        public async Task<bool> DeleteProductImage(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }

            ProductImg? response =await _productImageRepository.GetProductImageByImageID(imageID.Value);
            if (response is null)
            {
                return false;
            }
            await _productImageRepository.DeleteProductImage(imageID.Value);
            return true;
        }

        public async Task<List<ProductImageResponse>>? GetAllProductImages()
        {
            return (await _productImageRepository.GetAllProductImages()).Select(t => t.ToProductImgResponse()).ToList();
        }

        public async Task<ProductImageResponse>? GetProductImageByImageID(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }
            ProductImg? productImg = await _productImageRepository.GetProductImageByImageID(imageID.Value);
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
            List<ProductImg>? response =await _productImageRepository.GetProductImagesByProductID(productId.Value);
            if (response == null)
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
            ProductImg? response =await _productImageRepository.GetProductImageByImageID(updateRequest.ImageID);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            await _productImageRepository.UpdateProductImage(response);
            return response.ToProductImgResponse();
        }
    }

}

