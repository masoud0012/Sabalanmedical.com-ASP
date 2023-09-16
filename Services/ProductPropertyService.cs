﻿using Entities;
using IRepository;
using IRepository2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceContracts;
using ServiceContracts.DTO.ProductPropertyDTO;
using Services.Helpers;
using System;


namespace Services
{
    public class ProductPropertyService : IProductPropertyService
    {
        private readonly IProductPropertyRepository _productPropertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductPropertyService(IProductPropertyRepository productPropertyRepository, IUnitOfWork unitOfWork)
        {
            _productPropertyRepository = productPropertyRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<ProductPropertyResponse>? AddProductProperty(ProductPropertyAddRequest request)
        {

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            await _productPropertyRepository.Add(request.ToProductProperty());
            await _unitOfWork.SaveChanges();
            return request.ToProductProperty().ToProductPropertyResponse();
        }

        public async Task<bool> DeleteProductProperty(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property = await _productPropertyRepository.GetById(propertyId.Value);
            if (property == null) { return false; }
            await _productPropertyRepository.Delete(property);
            await _unitOfWork.SaveChanges();
            return true;
        }

        public async Task<List<ProductPropertyResponse>>? GetAllProductProperty()
        {
            List<ProductProperty>? productProperties = (await _productPropertyRepository.GetAllAsync(0,100)).ToList();
            return productProperties.Select(t => t.ToProductPropertyResponse()).ToList();
        }

        public async Task<List<ProductPropertyResponse>>? GetProductPropertiesByProductID(Guid? productId)
        {
            List<ProductProperty>? properties;
            if (productId == null)
            {
                throw new ArgumentException(nameof(productId));
            }
            List<ProductProperty>? productProperties = (await _productPropertyRepository.GetByProductID(productId.Value)).ToList();
            return productProperties.Select(t => t.ToProductPropertyResponse()).ToList();
        }

        public async Task<ProductPropertyResponse>? GetProductPropertyByPropertyID(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property = await _productPropertyRepository.GetById(propertyId.Value);
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
            ProductProperty? property = await _productPropertyRepository.GetById(updateRequest.propertyID);
            if (property == null)
            {
                throw new ArgumentException("No property was found!");
            }
            await _productPropertyRepository.Update(updateRequest.ToProductProperty());
            await _unitOfWork.SaveChanges();
            return property.ToProductPropertyResponse();
        }
    }
}
