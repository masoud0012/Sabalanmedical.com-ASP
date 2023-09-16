using Entities;
using IRepository;
using IRepository2;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO.ProductTypeDTO;

namespace Services
{
    public class ProductTypesService:IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductTypeRepository _productTypeRepository;
        public ProductTypesService(IProductTypeRepository productTypeRepository,IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
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
            var type = await _productTypeRepository.GetProductTypeByName(productTypeAddRequest.TypeNameEN);
            if (type!=null)
            {
                throw new ArgumentException($"{productTypeAddRequest.TypeNameEN} or {productTypeAddRequest.TypeNameFr} is alraedy excisted");
            }

            ProductType productType = productTypeAddRequest.ToProductType();
            await _productTypeRepository.Add(productType);
            await _unitOfWork.SaveChanges();
            return productType.ToProductTypeResponse();
        }

        public async Task<ProductTypeResponse>? GetProductTypeByID(Guid? typeId)
        {
            if (typeId == null)
            {
                throw new ArgumentNullException(nameof(typeId));
            }
            ProductType? response = await _productTypeRepository.GetById(typeId.Value);
            if (response is null)
            {
                throw new ArgumentException(nameof(response));
            }
            return response.ToProductTypeResponse();
        }

        public async Task<List<ProductTypeResponse>>? GetAllProductTypes()
        {
            List<ProductType> productTypes = (await _productTypeRepository.GetAllAsync()).ToList();
            return productTypes.Select(t => t.ToProductTypeResponse()).ToList();
        }

        public async Task<ProductTypeResponse>? GetProductTypeByName(string? name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            ProductType type = await _productTypeRepository.GetProductTypeByName(name);
           
            return type.ToProductTypeResponse();
        }
    }
};