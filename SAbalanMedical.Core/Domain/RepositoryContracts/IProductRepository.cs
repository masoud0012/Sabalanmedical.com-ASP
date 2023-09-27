using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IProductRepository : IRepositoryGeneric<Product>
    {
        /// <summary>
        /// Returns an object of product based on a Url property
        /// </summary>
        /// <param name="productUrl as string">productUrl</param>
        /// <returns>Product</returns>
        Task<Product?> GetProductByProductUrl(string productUrl);

        /// <summary>
        /// Returns a list of products matched by search attribute and search key
        /// </summary>
        /// <param name="searchBy">Product properties</param>
        /// <param name="searchKey">String Keyword</param>
        /// <returns>a list a product</returns>
        Task<List<Product>?> GetFilteredProduct(Expression<Func<Product, bool>> predicate, int skip = 0, int take = 50);

        /// <summary>
        /// Returns a list of products matched by name
        /// </summary>
        /// <param name="productNameEN">productNameEN</param>
        /// <returns>Product</returns>
        Task<Product?> GetProductByName(string productNameEN, string productNameFr);

        /// <summary>
        /// Returns a list of Product whose typeId is matched with the given typeId
        /// </summary>
        /// <param name="type">typeId</param>
        /// <returns>A list of Product</returns>
        public Task<List<Product>> GetProductsByTypeId(Guid typeId);
        /// <summary>
        /// Returns a list of Product whose typeId is matched with the given typeName
        /// </summary>
        /// <param name="type">typeNameEN</param>
        /// <returns>A list of Product</returns>
        public Task<List<Product>> GetProductsByTypeName(string typeName);
    }
}
