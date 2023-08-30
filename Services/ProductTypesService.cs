using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO.ProductTypeDTO;

namespace Services
{
    public class ProductTypesService : IProductTypeService
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductTypesService(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
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
            if (_sabalanDbContext.ProductTypes.Count(t=>t.TypeNameEN==productTypeAddRequest.TypeNameEN
            || t.TypeNameFr == productTypeAddRequest.TypeNameFr)>0)
            {
                throw new ArgumentException($"{productTypeAddRequest.TypeNameEN} or {productTypeAddRequest.TypeNameFr} is alraedy excisted");
            }
            ProductType productType = productTypeAddRequest.ToProductType();
            _sabalanDbContext.ProductTypes.Add(productType);
            await _sabalanDbContext.SaveChangesAsync();
            return productType.ToProductTypeResponse();
        }

        public async Task<ProductTypeResponse>? GetProductTypeByID(Guid? typeId)
        {
            if (typeId == null)
            {
                return null;
            }
            ProductType? response =await _sabalanDbContext.ProductTypes
                .FirstOrDefaultAsync(temp => temp.TypeId == typeId);
            return response?.ToProductTypeResponse();
        }

        public async Task<List<ProductTypeResponse>>? GetAllProductTypes()
        {
            List<ProductType> productTypes = await _sabalanDbContext.sp_GetAllProductTypes();
           // List<ProductType> productTypes =await _sabalanDbContext.ProductTypes.ToListAsync();
            return productTypes.Select(temp => temp.ToProductTypeResponse()).ToList();
        }
    }
};