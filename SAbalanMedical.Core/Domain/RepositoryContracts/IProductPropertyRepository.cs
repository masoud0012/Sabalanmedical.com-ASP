using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IProductPropertyRepository: IRepositoryGeneric<ProductProperty>
    {
        /// <summary>
        /// Return all Properties of a specific Product
        /// </summary>
        /// <param name="productId">ProductID</param>
        /// <returns>A list of ProductProperty</returns>
        Task<IQueryable<ProductProperty>>? GetByProductID(Guid productId);
    }
}
