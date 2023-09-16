using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
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
        public RepositoryGeneric(DbContext context)
        {
            this._context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> Add(TEntity obj)
        {
            await _dbSet.AddAsync(obj);
            return obj;
        }

        public Task<bool> Delete(TEntity obj)
        {
            _dbSet.Remove(obj);
            return Task.FromResult(true);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(int start = 0, int length = 50)
        {
            var data = _dbSet.Skip(start).Take(length).AsQueryable();
            return data;
        }

        public async Task<TEntity?> GetById(Guid guid)
        {
            return _dbSet.Single(t => t.Id == guid);
        }

        public async Task<TEntity?> Update(TEntity obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;
            _dbSet.Update(obj);
            return obj;
        }
    }
}