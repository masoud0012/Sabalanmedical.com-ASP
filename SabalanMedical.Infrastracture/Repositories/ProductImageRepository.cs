using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductImageRepository> _logger;
        public ProductImageRepository(SabalanDbContext context, ILogger<ProductImageRepository> logger) :base(context, logger)
        {
            _dbSet=context.Set<ProductImg>();   
            _logger=logger;
        }

        public async Task<IQueryable<ProductImg>>? GetByProductID(Guid productId)
        {
           return  _dbSet.Where(t => t.ProductID == productId).AsQueryable();
        }
    }
}
