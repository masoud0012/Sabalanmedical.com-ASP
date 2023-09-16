using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository2
{
    public interface IProductImgRepository: IRepositoryGeneric<ProductImg>
    {
        /// <summary>
        /// Return a list of all images of a specific Product
        /// </summary>
        /// <param name="productId">ProductId</param>
        /// <returns>A List of Objects as ProductImg</returns>
        Task<IQueryable<ProductImg>>? GetByProductID(Guid productId);
    }
}
