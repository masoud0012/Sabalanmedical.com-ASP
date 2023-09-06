using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Linq.Expressions;

namespace RepositoryServices
{
    public class ProductRepository : IProductRepository
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductRepository(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }

        public async Task<Product> AddProduct(Product product)
        {
            await _sabalanDbContext.Products.AddAsync(product);
            await _sabalanDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _sabalanDbContext.Products.Remove(product);
            int rows = await _sabalanDbContext.SaveChangesAsync();
            return rows > 0;
        }

        public Task<List<Product>> GetAllProducts()
        {
            return _sabalanDbContext.Products.Include("ProductType").Include("ProductImages")
                .Include("ProductDescriptions")
                .Include("ProductProperties").ToListAsync();
        }

        public async Task<List<Product>?> GetFilteredProduct(Expression<Func<Product, bool>> predicate)
        {
            List<Product>? products = await _sabalanDbContext.Products.Where(predicate)
                  .Include("ProductType").Include("ProductImages").Include("ProductDescriptions")
                  .Include("ProductProperties")
                  .ToListAsync();
            return products;
        }

        public async Task<Product?> GetProductById(Guid guid)
        {
            return await _sabalanDbContext.Products.Include("ProductType").Include("ProductDescriptions")
                              .Include("ProductImages").Include("ProductProperties").FirstOrDefaultAsync(t => t.ProductID == guid);
        }

        public async Task<Product?> GetProductByName(string? productNameEN, string? productNameFr)
        {
            return await _sabalanDbContext.Products.FirstOrDefaultAsync
                    (t => t.ProductNameEn == productNameEN || t.ProductNameFr == productNameFr);
        }

        public async Task<Product?> GetProductByProductUrl(string productUrl)
        {
            return await _sabalanDbContext.Products.Include("ProductType").Include("ProductDescription")
                      .Include("ProductImg").Include("ProductProperties")
                      .FirstOrDefaultAsync(t => t.ProductUrl == productUrl);
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? result = await _sabalanDbContext.Products.FirstOrDefaultAsync(t => t.ProductID == product.ProductID);
            if (result is null)
            {
                return product;
            }
            result.ProductNameEn = product.ProductNameEn;
            result.ProductNameFr = product.ProductNameFr;
            result.TypeId = product.TypeId;
            result.isManufactured = product.isManufactured;
            result.isHotSale = product.isHotSale;
            result.ProductUrl = product.ProductUrl;
            await _sabalanDbContext.SaveChangesAsync();
            return result;
        }
    }
}