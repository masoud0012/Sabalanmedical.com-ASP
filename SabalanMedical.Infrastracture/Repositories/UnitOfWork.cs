using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryServices
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public UnitOfWork(SabalanDbContext sabalanDbContext)
        {
            this._sabalanDbContext = sabalanDbContext;
        }
        public void IDisposable()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                await _sabalanDbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
