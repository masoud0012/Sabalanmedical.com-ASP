using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace RepositoryServices
{
    public class ProductTypeRepository : RepositoryGeneric<ProductType, SabalanDbContext>,IProductTypeRepository
    {
        private readonly ILogger<ProductTypeRepository> _logger;
        private readonly DbSet<ProductType> _dbSet;
        public ProductTypeRepository(SabalanDbContext context, ILogger<ProductTypeRepository> logger) : base(context,logger)
        {
            _dbSet=context.Set<ProductType>();
            _logger = logger;
        }

        public async Task<ProductType>? GetProductTypeByName(string name)
        {
            _logger.LogInformation("GetTypeByName method was executed");
            _logger.LogDebug($"- {name} - is the type name to be searched");
            return await _dbSet.SingleOrDefaultAsync(t => t.TypeNameEN == name);
        }
    }
}
