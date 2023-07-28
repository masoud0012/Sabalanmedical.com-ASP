using Entities;
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
        public ProductPropertyResponse? AddProductProperty(ProductPropertyAddRequest request)
        {

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            _sabalanDbContext.ProductProperties.Add(request.ToProductProperty());
            _sabalanDbContext.SaveChanges();
            return request.ToProductProperty().ToProductPropertyResponse();
        }

        public bool DeleteProductProperty(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property = _sabalanDbContext.ProductProperties.FirstOrDefault(t => t.propertyID == propertyId);
            if (property == null) { return false; }
            _sabalanDbContext.ProductProperties.Remove(property);
            _sabalanDbContext.SaveChanges();
            return true;
        }

        public List<ProductPropertyResponse>? GetAllProductProperty()
        {
            return _sabalanDbContext.ProductProperties.Select(t => t.ToProductPropertyResponse()).ToList();
        }

        public List<ProductPropertyResponse>? GetProductPropertiesByProductID(Guid? productId)
        {
            List<ProductProperty>? properties;
            if (productId==null)
            {
                throw new ArgumentException(nameof(productId));
            }
            properties = _sabalanDbContext.ProductProperties.Where(t => t.ProductID == productId).ToList();
            return properties.Select(t=>t.ToProductPropertyResponse()).ToList();
        }

        public ProductPropertyResponse? GetProductPropertyByPropertyID(Guid? propertyId)
        {
            if (propertyId is null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            ProductProperty? property = _sabalanDbContext.ProductProperties.FirstOrDefault(t => t.propertyID == propertyId);
            if (property == null)
            {
                throw new ArgumentException("The product property was not found!");
            }
            return property.ToProductPropertyResponse();
        }

        public ProductPropertyResponse? UpdateProductProperty(ProductPropertyUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ValidationHelper.ModelValidation(updateRequest);
            ProductProperty? property = _sabalanDbContext.ProductProperties.FirstOrDefault(t => t.propertyID == updateRequest.propertyID);
            if (property == null)
            {
                throw new ArgumentException("No property was found!");
            }
            property.propertyID = updateRequest.propertyID;
            property.ProductID = updateRequest.ProductID;
            property.PropertyTitle = updateRequest.PropertyTitle;
            property.PropertyDetail = updateRequest.PropertyDetail;
            _sabalanDbContext.SaveChanges();
            return property.ToProductPropertyResponse();
        }
    }
}
