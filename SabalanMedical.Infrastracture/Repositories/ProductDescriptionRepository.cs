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
    public class ProductDescriptionRepository:RepositoryGeneric<ProductDesc,SabalanDbContext>,IProductDescRepository
    {
        private readonly DbSet<ProductDesc> _dbSet;
        private readonly ILogger<ProductDescriptionRepository> _logger; 
        public ProductDescriptionRepository(SabalanDbContext context, ILogger<ProductDescriptionRepository> logger) :base(context, logger)
        {
            _dbSet=context.Set<ProductDesc>();
            _logger=logger;
        }

        public async Task<IQueryable<ProductDesc>>? GetByProductID(Guid productID)
        {
            return _dbSet.Where(t=>t.ProductID==productID).AsQueryable();
        }
    }
}
