using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO.ProductTypeDTO;

namespace Services
{
    public class ProductTypesService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        public ProductTypesService( IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }
        public async Task<ProductTypeResponse> AddProductType(ProductTypeAddRequest? productTypeAddRequest)
        {
            if (productTypeAddRequest is null)
            {
                throw new ArgumentNullException(nameof(productTypeAddRequest));
            }
            if (productTypeAddRequest.TypeNameEN is null && productTypeAddRequest.TypeNameFr is null)
            {
                throw new ArgumentException(nameof(productTypeAddRequest.TypeNameEN) + "or" +
                    nameof(ProductTypeAddRequest.TypeNameFr));
            }
            List<ProductType> allTypes=await _productTypeRepository.GetAllProductTypes();
            if (allTypes.Count(t => t.TypeNameEN == productTypeAddRequest.TypeNameEN
            || t.TypeNameFr == productTypeAddRequest.TypeNameFr) > 0)
            {
                throw new ArgumentException($"{productTypeAddRequest.TypeNameEN} or {productTypeAddRequest.TypeNameFr} is alraedy excisted");
            }

            ProductType productType = productTypeAddRequest.ToProductType();
            await _productTypeRepository.AddProductType(productType);

            return productType.ToProductTypeResponse();
        }

        public async Task<ProductTypeResponse>? GetProductTypeByID(Guid? typeId)
        {
            if (typeId == null)
            {
                throw new ArgumentNullException(nameof(typeId));
            }
            ProductType? response = await _productTypeRepository.GetProductTypeByID(typeId);
            if (response is null)
            {
                throw new ArgumentException(nameof(response));
            }
            return response.ToProductTypeResponse();
        }

        public async Task<List<ProductTypeResponse>>? GetAllProductTypes()
        {
            // List<ProductType> productTypes = await _sabalanDbContext.sp_GetAllProductTypes();
            List<ProductType> productTypes = await _productTypeRepository.GetAllProductTypes();
            return productTypes.Select(t => t.ToProductTypeResponse()).ToList();
        }
    }
};