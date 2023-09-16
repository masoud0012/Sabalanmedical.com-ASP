using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IProductTypeRepository: IRepositoryGeneric<ProductType>
    {
        /// <summary>
        /// Get ProductTypeBy name in EN
        /// </summary>
        /// <param name="name">String Name</param>
        /// <returns>Object ProductType</returns>
        Task<ProductType>? GetProductTypeByName(string name);
    }
}
