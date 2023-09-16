using Entities;
using IRepository;
using IRepository;
using Microsoft.EntityFrameworkCore;
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
        private DbSet<Product> _dbSet;
        public ProductRepository(SabalanDbContext sabalanDbContext) : base(sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
            _dbSet = _sabalanDbContext.Set<Product>();
        }
        public async Task<List<Product>?> GetFilteredProduct(Expression<Func<Product, bool>> predicate, int start, int length)
        {
            return await _dbSet.Where(predicate).Skip(start).Take(length).Include("ProductType").Include("ProductImages").Include("ProductDescriptions")
                  .Include("ProductProperties").ToListAsync();
        }
        
        public Task<Product?> GetProductByName(string productNameEN, string productNameFr)
        {
            return _dbSet.FirstOrDefaultAsync(t => t.ProductNameEn == productNameEN && t.ProductNameFr == productNameFr);
        }

        public Task<Product?> GetProductByProductUrl(string productUrl)
        {
            return _dbSet.FirstOrDefaultAsync(t => t.ProductNameEn == productUrl);
        }
    }
}
