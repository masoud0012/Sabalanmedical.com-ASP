using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IProductDescRepository: IRepositoryGeneric<ProductDesc>
    {
        /// <summary>
        /// Returns all descriptions of a product based on product id
        /// </summary>
        /// <param name="productID">ProductId</param>
        /// <returns>A list of Product Descriptions</returns>
        Task<IQueryable<ProductDesc>>? GetByProductID(Guid productID);
    }
}
