using Entities;
using IRepository;
using IRepository;
using ServiceContracts;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductsDTO;
using Services.Helpers;
using System;

namespace Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImgRepository _productImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductImageService(IProductImgRepository productImageRepository, IUnitOfWork unitOfWork)
        {
            _productImageRepository = productImageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductImageResponse>? AddProductImage(ProductImageAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductImg productImage = request.ToProductImage();
            await _productImageRepository.Add(request.ToProductImage());
            await _unitOfWork.SaveChanges();
            return productImage.ToProductImgResponse();
        }

        public async Task<bool> DeleteProductImage(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }

            ProductImg? response =await _productImageRepository.GetById(imageID.Value);
            if (response is null)
            {
                return false;
            }
            await _productImageRepository.Delete(response);
            await _unitOfWork.SaveChanges();
            return true;
        }

        public async Task<List<ProductImageResponse>>? GetAllProductImages()
        {
            return (await _productImageRepository.GetAllAsync()).Select(t => t.ToProductImgResponse()).ToList();
        }

        public async Task<ProductImageResponse>? GetProductImageByImageID(Guid? imageID)
        {
            if (imageID is null)
            {
                throw new ArgumentNullException(nameof(imageID));
            }
            ProductImg? productImg = await _productImageRepository.GetById(imageID.Value);
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
            List<ProductImg>? response =(await _productImageRepository.GetByProductID(productId.Value)).ToList();
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
            ProductImg? response =await _productImageRepository.GetById(updateRequest.Id);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            await _productImageRepository.Update(response);
            await _unitOfWork.SaveChanges();
            return response.ToProductImgResponse();
        }
    }

}

