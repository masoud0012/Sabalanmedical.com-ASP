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
    public class ProductPropertyRepository : RepositoryGeneric<ProductProperty, SabalanDbContext>, IProductPropertyRepository
    {
        private readonly DbSet<ProductProperty> _dbSet;
        private readonly ILogger<ProductPropertyRepository> _logger;
        public ProductPropertyRepository(SabalanDbContext context, ILogger<ProductPropertyRepository> logger) : base(context,logger)
        {
            _dbSet=context.Set<ProductProperty>();
            _logger=logger;
        }

        public async Task<IQueryable<ProductProperty>>? GetByProductID(Guid productId)
        {
            return  _dbSet.Where(t => t.ProductID == productId).AsQueryable();   
        }
    }
}
