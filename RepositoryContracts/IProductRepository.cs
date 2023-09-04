using Entities;
using System.Linq.Expressions;
using System;

namespace RepositoryContracts
{
    public interface IProductRepository
    {
        /// <summary>
        /// To add new product request object to database
        /// </summary>
        /// <param name="productAddRequest">ProductAddRequest</param>
        /// <returns>Returns new product object</returns>
        Task<Product> AddProduct(Product product);

        /// <summary>
        /// To return a list of product objects as product objects
        /// </summary>
        /// <returns>a list of product</returns>
        Task<List<Product>> GetAllProducts();

        /// <summary>
        /// search for a product object based on a guid ID
        /// </summary>
        /// <param name="guid">ProductID</param>
        /// <returns>returns a product object as product</returns>
        Task<Product?> GetProductById(Guid guid);
        Task<Product?> GetProductByProductUrl(string productUrl);

        /// <summary>
        /// Returns a list of products matched by search attribute and search key
        /// </summary>
        /// <param name="searchBy">Product properties</param>
        /// <param name="searchKey">String Keyword</param>
        /// <returns>a list a product</returns>
        Task<List<Product>?> GetFilteredProduct(Expression<Func<Product,bool>> predicate);
        /// <summary>
        /// Delete a Product object based on a ProductID
        /// </summary>
        /// <param name="guid">ProductID</param>
        /// <returns>returns true if the object was deleted else false</returns>
        Task<bool> DeleteProduct(Product product);

        Task<Product?> GetProductByName(string productNameEN,string productNameFr);

        Task<Product?> UpdateProduct(Product product);
    }
}
