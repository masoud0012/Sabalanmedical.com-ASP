using ServiceContracts.DTO.ProductTypeDTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represent Bussiness logic for manipulating Product Types
    /// </summary>
    public interface IProductTypeService
    {
        /// <summary>
        /// Adds a new Product type object to the list of product types
        /// </summary>
        /// <param name="productTypeAddRequest">product type object to be added</param>
        /// <returns>returns the productType object after adding to database</returns>
        Task<ProductTypeResponse> AddProductType(ProductTypeAddRequest? productTypeAddRequest);
        /// <summary>
        /// Returns all productTypes
        /// </summary>
        /// <returns>returns all product types as producttyperespnse</returns>
        Task<List<ProductTypeResponse>>? GetAllProductTypes();

        /// <summary>
        /// Get productType based on an ID
        /// </summary>
        /// <returns>returns productType as a productTypeResponse</returns>
        Task<ProductTypeResponse>? GetProductTypeByID(Guid? guid);
    }
    
}