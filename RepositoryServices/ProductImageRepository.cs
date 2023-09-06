using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace RepositoryServices
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductImageRepository(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }
        public async Task<ProductImg>? AddProductImage(ProductImg request)
        {
            await _sabalanDbContext.ProductImgs.AddAsync(request);
            await _sabalanDbContext.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteProductImage(Guid ImageId)
        {
            ProductImg? productImg = await _sabalanDbContext.ProductImgs.FirstOrDefaultAsync(t => t.ImageID == ImageId);
            _sabalanDbContext.Remove(productImg);
            return await _sabalanDbContext.SaveChangesAsync() > 0;

        }

        public async Task<List<ProductImg>>? GetAllProductImages()
        {
            return await _sabalanDbContext.ProductImgs.ToListAsync();
        }

        public async Task<ProductImg>? GetProductImageByImageID(Guid ImageId)
        {
            return await _sabalanDbContext.ProductImgs.FirstOrDefaultAsync(t => t.ImageID == ImageId);
        }

        public async Task<List<ProductImg>>? GetProductImagesByProductID(Guid productId)
        {
            return await _sabalanDbContext.ProductImgs.Where(t => t.ProductID == productId).ToListAsync();
        }

        public async Task<ProductImg>? UpdateProductImage(ProductImg updateRequest)
        {
            ProductImg productImg = await _sabalanDbContext.ProductImgs.FirstOrDefaultAsync(t => t.ImageID == updateRequest.ImageID);
            productImg.ImageUrl = updateRequest.ImageUrl;
            await _sabalanDbContext.SaveChangesAsync();
            return updateRequest;
        }
    }
}
