using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO.ProductDescriptionDTO;
using Services.Helpers;
using System;

namespace Services
{
    public class ProductDescService : IProductDescService
    {
        private readonly IProductDescriptionRepository _productDescriptionRepository;
        public ProductDescService(IProductDescriptionRepository productDescriptionRepository)
        {
            _productDescriptionRepository = productDescriptionRepository;
        }
        public async Task<ProductDescResponse>? AddProductDesc(ProductDescAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductDesc productDesc = request.ToProductDesc();
            await _productDescriptionRepository.AddProductDesc(productDesc);
            return productDesc.ToProductDescResponse();
        }

        public async Task<bool> DeleteProductDesc(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? response = await _productDescriptionRepository.GetProductDescByDescID(id.Value);
            if (response is null)
            {
                return false;
            }
            await _productDescriptionRepository.DeleteProductDesc(response.ProductID);
            return true;
        }

        public async Task<List<ProductDescResponse>>? GetAllProductDesc()
        {
            return (await _productDescriptionRepository.GetAllProductDesc()).Select(t => t.ToProductDescResponse()).ToList();
        }

        public async Task<ProductDescResponse>? GetProductDescByDescID(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? productDesc = await _productDescriptionRepository.GetProductDescByDescID(id.Value);
            if (productDesc == null)
            {
                throw new ArgumentException("No Description was found!");
            }
            return productDesc.ToProductDescResponse();
        }

        public async Task<List<ProductDescResponse>>? GetProductDescByProductID(Guid? productID)
        {
            List<ProductDesc>? productDescResponses = await _productDescriptionRepository.GetProductDescByProductID(productID.Value);
            return productDescResponses.Select(t => t.ToProductDescResponse()).ToList();
        }

        public async Task<ProductDescResponse>? UpdateProductDesc(ProductDescUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ProductDesc? response = await _productDescriptionRepository.GetProductDescByDescID(updateRequest.DesctiptionID);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            await _productDescriptionRepository.UpdateProductDesc(response);
            return response.ToProductDescResponse();
        }
    }
}
