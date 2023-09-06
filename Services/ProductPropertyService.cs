using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO.ProductPropertyDTO;
using Services.Helpers;
using System;


namespace Services
{
    public class ProductPropertyService : IProductPropertyService
    {
        private readonly IProductPropertiesRepository _productPropertyRepository;
        public ProductPropertyService(IProductPropertiesRepository productPropertyRepository)
        {
            _productPropertyRepository = productPropertyRepository;
        }
        public async Task<ProductPropertyResponse>? AddProductProperty(ProductPropertyAddRequest request)
        {

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            await _productPropertyRepository.AddProductProperty(request.ToProductProperty());
            return request.ToProductProperty().ToProductPropertyResponse();
        }

        public async Task<bool> DeleteProductProperty(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property =await _productPropertyRepository.GetProductPropertyByPropertyID(propertyId.Value);
            if (property == null) { return false; }
            await _productPropertyRepository.DeleteProductProperty(property);
            return true;
        }

        public async Task<List<ProductPropertyResponse>>? GetAllProductProperty()
        {
            List<ProductProperty>? productProperties = await _productPropertyRepository.GetAllProductProperty();
            return productProperties.Select(t => t.ToProductPropertyResponse()).ToList();
        }

        public async Task<List<ProductPropertyResponse>>? GetProductPropertiesByProductID(Guid? productId)
        {
            List<ProductProperty>? properties;
            if (productId==null)
            {
                throw new ArgumentException(nameof(productId));
            }
            List<ProductProperty>? productProperties = await _productPropertyRepository.GetProductPropertiesByProductID(productId.Value);
            return productProperties.Select(t=>t.ToProductPropertyResponse()).ToList();
        }

        public async Task<ProductPropertyResponse>? GetProductPropertyByPropertyID(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property = await _productPropertyRepository.GetProductPropertyByPropertyID(propertyId.Value);
            if (property == null)
            {
                throw new ArgumentException("The product property was not found!");
            }
            return property.ToProductPropertyResponse();
        }

        public async Task<ProductPropertyResponse>? UpdateProductProperty(ProductPropertyUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ValidationHelper.ModelValidation(updateRequest);
            ProductProperty? property =await _productPropertyRepository.GetProductPropertyByPropertyID(updateRequest.propertyID);
            if (property == null)
            {
                throw new ArgumentException("No property was found!");
            }
            await _productPropertyRepository.UpdateProductProperty(updateRequest.ToProductProperty());
            return property.ToProductPropertyResponse();
        }
    }
}
