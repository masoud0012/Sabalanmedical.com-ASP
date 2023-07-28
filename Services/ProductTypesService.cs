using Entities;
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
        public ProductTypeResponse AddProductType(ProductTypeAddRequest? productTypeAddRequest)
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
            _sabalanDbContext.SaveChanges();
            return productType.ToProductTypeResponse();
        }

        public ProductTypeResponse? GetProductTypeByID(Guid? typeId)
        {
            if (typeId == null)
            {
                return null;
            }
            ProductType? response = _sabalanDbContext.ProductTypes.FirstOrDefault(temp => temp.TypeId == typeId);
            return response?.ToProductTypeResponse();
        }

        public List<ProductTypeResponse>? GettAllProductTypes()
        {
            return _sabalanDbContext.ProductTypes.Select(temp => temp.ToProductTypeResponse()).ToList();
        }
    }
};