using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO.ProductPropertyDTO;
using Services.Helpers;
using System;


namespace Services
{
    public class ProductPropertyService : IProductPropertyService
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductPropertyService(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }
        public async Task<ProductPropertyResponse>? AddProductProperty(ProductPropertyAddRequest request)
        {

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            _sabalanDbContext.ProductProperties.Add(request.ToProductProperty());
            await _sabalanDbContext.SaveChangesAsync();
            return request.ToProductProperty().ToProductPropertyResponse();
        }

        public async Task<bool> DeleteProductProperty(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property = _sabalanDbContext.ProductProperties.FirstOrDefault(t => t.propertyID == propertyId);
            if (property == null) { return false; }
            _sabalanDbContext.ProductProperties.Remove(property);
            await _sabalanDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductPropertyResponse>>? GetAllProductProperty()
        {
            return await _sabalanDbContext.ProductProperties.
                Select(t => t.ToProductPropertyResponse()).ToListAsync();
        }

        public async Task<List<ProductPropertyResponse>>? GetProductPropertiesByProductID(Guid? productId)
        {
            List<ProductProperty>? properties;
            if (productId==null)
            {
                throw new ArgumentException(nameof(productId));
            }
            properties =await _sabalanDbContext.ProductProperties.
                Where(t => t.ProductID == productId).ToListAsync();
            return properties.Select(t=>t.ToProductPropertyResponse()).ToList();
        }

        public async Task<ProductPropertyResponse>? GetProductPropertyByPropertyID(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property =await _sabalanDbContext.ProductProperties.FirstOrDefaultAsync(t => t.propertyID == propertyId);
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
            ProductProperty? property =await _sabalanDbContext.ProductProperties.FirstOrDefaultAsync(t => t.propertyID == updateRequest.propertyID);
            if (property == null)
            {
                throw new ArgumentException("No property was found!");
            }
            property.propertyID = updateRequest.propertyID;
            property.ProductID = updateRequest.ProductID;
            property.PropertyTitle = updateRequest.PropertyTitle;
            property.PropertyDetail = updateRequest.PropertyDetail;
           await _sabalanDbContext.SaveChangesAsync();
            return property.ToProductPropertyResponse();
        }
    }
}
