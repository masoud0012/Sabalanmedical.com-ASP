using System.Linq.Expressions;
using Entities;

namespace IRepository
{
    public interface IRepositoryGeneric<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// To add new TEntity request object to database
        /// </summary>
        /// <param name="productAddRequest">ProductAddRequest</param>
        /// <returns>Returns new TEntity object</returns>
        Task<TEntity> Add(TEntity product);

        /// <summary>
        /// To return a list of TEntity objects 
        /// </summary>
        /// <returns>a list of TEntity</returns>
        Task<IQueryable<TEntity>> GetAllAsync(int start=0, int length=50);

        /// <summary>
        /// search for a TEntity object based on a guid ID
        /// </summary>
        /// <param name="guid">TEntity.ID</param>
        /// <returns>returns a TEntity object </returns>
        Task<TEntity?> GetById(Guid guid);

        /// <summary>
        /// To Remove TEntiry objectfrom database
        /// </summary>
        /// <param name="obj">TEntiry</param>
        /// <returns>Returns TRue if obj deleted else fals</returns>
        Task<bool> Delete(TEntity obj);
        /// <summary>
        /// To update obj of TEntiry from database
        /// </summary>
        /// <param name="obj">TEntity</param>
        /// <returns>Returns an object of TEntity</returns>
        Task<TEntity?> Update(TEntity obj);

 
    }
}