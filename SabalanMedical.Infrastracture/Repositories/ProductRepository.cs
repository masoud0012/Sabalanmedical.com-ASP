using Entities;
using IRepository;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryServices
{
    public class ProductRepository : RepositoryGeneric<Product, SabalanDbContext>, IProductRepository
    {
        private readonly SabalanDbContext _sabalanDbContext;
        private readonly ILogger<ProductRepository> _logger;
        private DbSet<Product> _dbSet;
        public ProductRepository(SabalanDbContext sabalanDbContext, ILogger<ProductRepository> logger) : base(sabalanDbContext, logger)
        {
            _sabalanDbContext = sabalanDbContext;
            _dbSet = _sabalanDbContext.Set<Product>();
            _logger = logger;
        }
        public async Task<List<Product>?> GetFilteredProduct(Expression<Func<Product, bool>> predicate, int start, int length)
        {
            return await _dbSet.Where(predicate).Skip(start).Take(length).Include("ProductType").Include("ProductImages").Include("ProductDescriptions")
                  .Include("ProductProperties").ToListAsync();
        }

        public Task<Product?> GetProductByName(string productNameEN, string productNameFr)
        {
            return _dbSet
                .Include(t => t.ProductType)
                .Include(t => t.ProductImages)
                .Include(t => t.ProductProperties)
                .Include(t => t.ProductDescriptions)
                .FirstOrDefaultAsync(t => t.ProductNameEn == productNameEN && t.ProductNameFr == productNameFr);
        }

        public async Task<Product?> GetProductByProductUrl(string productUrl)
        {
            var product = await _dbSet
                .Include(t => t.ProductType)
                .Include(t => t.ProductDescriptions)
                .Include(t => t.ProductProperties)
                .Include(t => t.ProductImages)
                .SingleOrDefaultAsync(t => t.ProductUrl == productUrl);
            return product;
        }

        public Task<List<Product>> GetProductsByTypeId(Guid typeId)
        {
            return _dbSet
                 .Include(t => t.ProductType)
                 .Include(t => t.ProductDescriptions)
                 .Include(t => t.ProductProperties)
                 .Include(t => t.ProductImages)
                 .Where(t => t.TypeId == typeId).ToListAsync();
        }

        public Task<List<Product>> GetProductsByTypeName(string typeName)
        {
            return _dbSet
               .Include(t => t.ProductType)
               .Include(t => t.ProductDescriptions)
               .Include(t => t.ProductProperties)
               .Include(t => t.ProductImages)
               .Where(t => t.ProductType.TypeNameEN == typeName).ToListAsync();
        }
    }
}
