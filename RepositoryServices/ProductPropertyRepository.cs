using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RepositoryContracts;

namespace RepositoryServices
{
    public class ProductPropertyRepository : IProductPropertiesRepository
    {
        private readonly Entities.SabalanDbContext _sabalanDbContext;
        public ProductPropertyRepository(Entities.SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }

        public async Task<ProductProperty>? AddProductProperty(ProductProperty request)
        {
            await _sabalanDbContext.ProductProperties.AddAsync(request);
            await _sabalanDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteProductProperty(ProductProperty productProperty)
        {
            _sabalanDbContext.ProductProperties.Remove(productProperty);
            return await _sabalanDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<ProductProperty>>? GetAllProductProperty()
        {
            return await _sabalanDbContext.ProductProperties.ToListAsync();
        }

        public async Task<List<ProductProperty>>? GetProductPropertiesByProductID(Guid productId)
        {
            return await _sabalanDbContext.ProductProperties.Where(t => t.ProductID == productId).ToListAsync();
        }

        public async Task<ProductProperty>? GetProductPropertyByPropertyID(Guid propertyId)
        {
            return await _sabalanDbContext.ProductProperties.FirstOrDefaultAsync(t => t.Id == propertyId);
        }

        public async Task<ProductProperty>? UpdateProductProperty(ProductProperty updateRequest)
        {
            ProductProperty productProperty = await _sabalanDbContext.ProductProperties.FirstOrDefaultAsync(t => t.Id == updateRequest.Id);
            productProperty.PropertyDetail = updateRequest.PropertyDetail;
            productProperty.PropertyTitle = updateRequest.PropertyTitle;
            _sabalanDbContext.SaveChangesAsync();
            return updateRequest;
        }
    }
}
