using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace RepositoryServices
{
    public class ProductDescriptionRepository : IProductDescriptionRepository
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductDescriptionRepository(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }
        public async Task<ProductDesc>? AddProductDesc(ProductDesc request)
        {
            _sabalanDbContext.ProductDescs.Add(request);
            await _sabalanDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteProductDesc(Guid id)
        {
            ProductDesc desc = await GetProductDescByDescID(id);
            _sabalanDbContext.ProductDescs.Remove(desc);
            await _sabalanDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDesc>>? GetAllProductDesc()
        {
            return await _sabalanDbContext.ProductDescs.ToListAsync();
        }

        public async Task<ProductDesc>? GetProductDescByDescID(Guid descID)
        {
            return await _sabalanDbContext.ProductDescs.FirstOrDefaultAsync(t => t.DesctiptionID == descID);
        }

        public async Task<List<ProductDesc>>? GetProductDescByProductID(Guid productID)
        {
            return await _sabalanDbContext.ProductDescs.Where(t => t.ProductID == productID).ToListAsync();
        }

        public async Task<ProductDesc>? UpdateProductDesc(ProductDesc updateRequest)
        {
            ProductDesc productDesc=await _sabalanDbContext.ProductDescs.FirstOrDefaultAsync(t => t.DesctiptionID == updateRequest.DesctiptionID);
            productDesc.DescTitle=updateRequest.DescTitle;
            productDesc.Description = updateRequest.Description;
            await _sabalanDbContext.SaveChangesAsync();
            return updateRequest;
        }
    }
}
