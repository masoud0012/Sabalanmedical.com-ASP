using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace RepositoryServices
{
    public abstract class RepositoryGeneric<TEntity, TContext> : IRepositoryGeneric<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ILogger<RepositoryGeneric<TEntity, TContext>> _logger;
        public RepositoryGeneric(DbContext context, ILogger<RepositoryGeneric<TEntity, TContext>> logger)
        {
            this._context = context;
            _dbSet = _context.Set<TEntity>();
            _logger = logger;
        }

        public virtual async Task<TEntity> Add(TEntity obj)
        {
            _logger.LogInformation($"Add method executed-TEntity={typeof(TEntity)}");
            await _dbSet.AddAsync(obj);
            _logger.LogInformation($"{obj.GetType().Name} was added");
            return obj;
        }

        public Task<bool> Delete(TEntity obj)
        {
            _logger.LogInformation($"Delete method executed-TEntity={typeof(TEntity)}");
            _dbSet.Remove(obj);
            _logger.LogDebug($"(obj.GetType().Name) was deleted");
            return Task.FromResult(true);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(int start, int length)
        {
            _logger.LogInformation($"GetAllAsync method executed-TEntity={typeof(TEntity)}");
            var data = _dbSet.Skip(start).Take(length).OrderBy(t => t.Id).AsQueryable();
            _logger.LogDebug($"{data.Count()} obj was found to be returned for {typeof(TEntity)}");
            return data;
        }

        public async Task<IQueryable<TEntity>> GetById(Guid guid)
        {

            _logger.LogInformation($"GetById method executed-TEntity={typeof(TEntity)}");
            return  _dbSet.Where(t => t.Id == guid).AsQueryable();
        }

        public async Task<TEntity?> Update(TEntity obj)
        {
            _logger.LogInformation($"Update method executed-TEntity={typeof(TEntity)}");
            _logger.LogDebug($"ready to update {obj.GetType().Name} object");
            _dbSet.Entry(obj).State = EntityState.Modified;
            _dbSet.Update(obj);
            return obj;
        }
    }
}