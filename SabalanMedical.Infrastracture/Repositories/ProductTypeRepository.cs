using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using RepositoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryServices
{
    public class ProductTypeRepository : RepositoryGeneric<ProductType, SabalanDbContext>,IProductTypeRepository
    {
        private readonly DbSet<ProductType> _dbSet;
        public ProductTypeRepository(SabalanDbContext context) : base(context)
        {
            _dbSet=context.Set<ProductType>();
        }

        public async Task<ProductType>? GetProductTypeByName(string name)
        {
            return await _dbSet.SingleOrDefaultAsync(t => t.TypeNameEN == name);
        }
    }
}
