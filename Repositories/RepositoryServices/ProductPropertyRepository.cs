using Entities;
using IRepository2;
using Microsoft.EntityFrameworkCore;
using RepositoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryServices2
{
    public class ProductPropertyRepository : RepositoryGeneric<ProductProperty, SabalanDbContext>, IProductPropertyRepository
    {
        private readonly DbSet<ProductProperty> _dbSet;
        public ProductPropertyRepository(SabalanDbContext context) : base(context)
        {
            _dbSet=context.Set<ProductProperty>();
        }

        public async Task<IQueryable<ProductProperty>>? GetByProductID(Guid productId)
        {
            return  _dbSet.Where(t => t.ProductID == productId).AsQueryable();   
        }
    }
}
