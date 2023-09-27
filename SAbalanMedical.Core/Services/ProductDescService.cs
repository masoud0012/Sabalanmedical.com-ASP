using Entities;
using IRepository;
using IRepository;
using ServiceContracts;
using ServiceContracts.DTO.ProductDescriptionDTO;
using Services.Helpers;
using System;

namespace Services
{
    public class ProductDescService : IProductDescService
    {
        private readonly IProductDescRepository _productDescriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductDescService(IProductDescRepository productDescriptionRepository, IUnitOfWork unitOfWork)
        {
            _productDescriptionRepository = productDescriptionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductDescResponse>? AddProductDesc(ProductDescAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductDesc productDesc = request.ToProductDesc();
            await _productDescriptionRepository.Add(productDesc);
            await _unitOfWork.SaveChanges();
            return productDesc.ToProductDescResponse();
        }

        public async Task<bool> DeleteProductDesc(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? response = await _productDescriptionRepository.GetById(id.Value);
            if (response is null)
            {
                return false;
            }
            await _productDescriptionRepository.Delete(response);
            await _unitOfWork.SaveChanges();
            return true;
        }

        public async Task<List<ProductDescResponse>>? GetAllProductDesc()
        {
            return (await _productDescriptionRepository.GetAllAsync(0, 50)).Select(t => t.ToProductDescResponse()).ToList();
        }

        public async Task<ProductDescResponse>? GetProductDescByDescID(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? productDesc = await _productDescriptionRepository.GetById(id.Value);
            if (productDesc == null)
            {
                throw new ArgumentException("No Description was found!");
            }
            return productDesc.ToProductDescResponse();
        }

        public async Task<List<ProductDescResponse>>? GetProductDescByProductID(Guid? productID)
        {
            List<ProductDesc>? productDescResponses = (await _productDescriptionRepository.GetByProductID(productID.Value)).ToList();
            return productDescResponses.Select(t => t.ToProductDescResponse()).ToList();
        }

        public async Task<ProductDescResponse>? UpdateProductDesc(ProductDescUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ProductDesc? response = await _productDescriptionRepository.GetById(updateRequest.DesctiptionID);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            response.ProductID = updateRequest.ProductID;
            response.DescTitle = updateRequest.DescTitle;
            response.Description = updateRequest.Description;
            await _productDescriptionRepository.Update(response);
            await _unitOfWork.SaveChanges();
            return response.ToProductDescResponse();
        }
    }
}
