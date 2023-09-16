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
    public class ProductImageRepository:RepositoryGeneric<ProductImg,SabalanDbContext>,IProductImgRepository
    {
        private readonly DbSet<ProductImg> _dbSet;
        public ProductImageRepository(SabalanDbContext context):base(context)
        {
            _dbSet=context.Set<ProductImg>();   
        }

        public async Task<IQueryable<ProductImg>>? GetByProductID(Guid productId)
        {
           return  _dbSet.Where(t => t.ProductID == productId).AsQueryable();
        }
    }
}
