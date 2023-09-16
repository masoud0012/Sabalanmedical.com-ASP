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
    public class ProductDescriptionRepository:RepositoryGeneric<ProductDesc,SabalanDbContext>,IProductDescRepository
    {
        private readonly DbSet<ProductDesc> _dbSet;
        public ProductDescriptionRepository(SabalanDbContext context):base(context)
        {
            _dbSet=context.Set<ProductDesc>();
        }

        public async Task<IQueryable<ProductDesc>>? GetByProductID(Guid productID)
        {
            return _dbSet.Where(t=>t.ProductID==productID).AsQueryable();
        }
    }
}
