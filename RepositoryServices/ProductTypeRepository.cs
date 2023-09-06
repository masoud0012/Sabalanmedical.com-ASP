using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryServices
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductTypeRepository(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }
        public async Task<ProductType> AddProductType(ProductType productType)
        {
            _sabalanDbContext.ProductTypes.Add(productType);
            await _sabalanDbContext.SaveChangesAsync();
            return productType;
        }

        public Task<bool> DeleteProductType(Guid typeID)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductType>? GetProductTypeByID(Guid? guid)
        {
            ProductType productType = await _sabalanDbContext.ProductTypes.FirstOrDefaultAsync(t => t.TypeId == guid);
            return productType;
        }

        public async Task<List<ProductType>> GetAllProductTypes()
        {
            return await _sabalanDbContext.ProductTypes.ToListAsync();
        }

        public Task<ProductType> UpdateProductType(ProductType productType)
        {
            throw new NotImplementedException();
        }
    }
}
