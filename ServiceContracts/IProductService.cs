using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.Enums;
using System;


namespace ServiceContracts
{
    public interface IProductService
    {
        /// <summary>
        /// To add new product request object to database
        /// </summary>
        /// <param name="productAddRequest">ProductAddRequest</param>
        /// <returns>Returns new ProductResponse object</returns>
        Task<ProductResponse> AddProduct(ProductAddRequest? productAddRequest);

        /// <summary>
        /// To return a list of product objects as productResponse objects
        /// </summary>
        /// <returns>a list of ProductResponse</returns>
        Task<List<ProductResponse>> GetAllProducts();

        /// <summary>
        /// search for a product object based on a guid ID
        /// </summary>
        /// <param name="guid">ProductID</param>
        /// <returns>returns a product object as ProductResponse</returns>
        Task<ProductResponse>? GetProductById(Guid? guid);
        Task<ProductResponse>? GetProductByProductUrl(string? productUrl);

        /// <summary>
        /// Returns a list of products matched by search attribute and search key
        /// </summary>
        /// <param name="searchBy">Product properties</param>
        /// <param name="searchKey">String Keyword</param>
        /// <returns>a list a ProductResponse</returns>
        Task<List<ProductResponse>>? GetFilteredProduct(string searchBy,string? searchKey);
        /// <summary>
        /// Sorts all Products based on property ond SortOrder Option Ascending or Descending
        /// </summary>
        /// <param name="allProducts">List of all ProductResponse</param>
        /// <param name="sortOrder">a list of sorted Products as ProductResponse</param>
        /// <returns></returns>
        Task<List<ProductResponse>> GetSortedProducts(List<ProductResponse> allProducts,string SortBy,SortOrderOptions sortOrder);
        /// <summary>
        /// Delete a Product object based on a ProductID
        /// </summary>
        /// <param name="guid">ProductID</param>
        /// <returns>returns true if the object was deleted else false</returns>
        Task<bool> DeleteProduct(Guid? guid);

        /// <summary>
        /// To edit a product object
        /// </summary>
        /// <param name="guid">ProductId</param>
        /// <returns>returns true if the product object was edited else fals</returns>
        Task<ProductResponse> UpdateProduct(ProductUpdateRequest? productUpdateRequest);

        /// <summary>
        /// convert all products to a csv file
        /// </summary>
        /// <returns>a csv file include all products response</returns>
        Task<MemoryStream> ProductToCsv();
    }
}
