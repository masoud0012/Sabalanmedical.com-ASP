using Entities;

namespace RepositoryContracts
{
    public interface IProductTypeRepository
    {
        Task<List<ProductType>> GetAllProductTypes();
        Task<ProductType> AddProductType(ProductType productType);
        Task<ProductType> UpdateProductType(ProductType productType);
        Task<bool> DeleteProductType(Guid typeID);
        Task<ProductType>? GetProductTypeByID(Guid? guid);

        Task<ProductType> GetProductTypeByName(string name);

    }
}