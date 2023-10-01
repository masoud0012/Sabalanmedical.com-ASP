using Entities;
using IRepository;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO.ProductTypeDTO;

namespace Services
{
    public class ProductTypesService:IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ILogger<ProductTypesService> _logger;
        private readonly IDiagnosticContext _diagnostic;
        public ProductTypesService(IProductTypeRepository productTypeRepository,IUnitOfWork unitOfWork,
            ILogger<ProductTypesService> logger,IDiagnosticContext diagnostic)
        {
            _diagnostic = diagnostic;
            _unitOfWork = unitOfWork;
            _productTypeRepository = productTypeRepository;
            _logger = logger;
        }
        public async Task<ProductTypeResponse> AddProductType(ProductTypeAddRequest? productTypeAddRequest)
        {
            _logger.LogInformation("AddProductType executed");
            if (productTypeAddRequest is null)
            {
                _logger.LogError("Request is null for AddProductType");
                throw new ArgumentNullException(nameof(productTypeAddRequest));
            }
            if (productTypeAddRequest.TypeNameEN is null && productTypeAddRequest.TypeNameFr is null)
            {
                _logger.LogError("TypeName is null for AddProductType");
                throw new ArgumentException(nameof(productTypeAddRequest.TypeNameEN) + "or" +
                    nameof(ProductTypeAddRequest.TypeNameFr));
            }
            _logger.LogDebug($"TypeEn: {productTypeAddRequest.TypeNameEN} and typeFr :{productTypeAddRequest.TypeNameFr}");
            var type = await _productTypeRepository.GetProductTypeByName(productTypeAddRequest.TypeNameEN);
            if (type!=null)
            {
                _logger.LogError($"{productTypeAddRequest.TypeNameEN} and {productTypeAddRequest.TypeNameFr} is already exist!");
                throw new ArgumentException($"{productTypeAddRequest.TypeNameEN} or {productTypeAddRequest.TypeNameFr} is alraedy excisted");
            }
            _logger.LogInformation($"{productTypeAddRequest.TypeNameEN} is ready to be added to database");
            ProductType productType = productTypeAddRequest.ToProductType();
            await _productTypeRepository.Add(productType);
            await _unitOfWork.SaveChanges();
            _logger.LogInformation($"{productTypeAddRequest.TypeNameEN} was added to database");
            _diagnostic.Set("AddedProduct", type);
            return productType.ToProductTypeResponse();
        }

        public async Task<ProductTypeResponse>? GetProductTypeByID(Guid? typeId)
        {
            _logger.LogInformation("GetProductTypeById executed");
            if (typeId == null)
            {
                _logger.LogError("Id is null");
                throw new ArgumentNullException(nameof(typeId));
            }
            ProductType? response = (await _productTypeRepository.GetById(typeId.Value)).SingleOrDefault();
            if (response is null)
            {
                _logger.LogError($"Id:{typeId} is not valid and did not find in database");
                throw new ArgumentException(nameof(response));
            }
            _logger.LogDebug($"{response.TypeNameEN} was found");
            _diagnostic.Set("GetTypeById", response);
            return response.ToProductTypeResponse();
        }

        public async Task<List<ProductTypeResponse>>? GetAllProductTypes()
        {
            _logger.LogInformation("GetAllTypes executed");
            List<ProductType> productTypes = (await _productTypeRepository.GetAllAsync()).ToList();
            _diagnostic.Set("AllTypes", productTypes);
            return productTypes.Select(t => t.ToProductTypeResponse()).ToList();
        }

        public async Task<ProductTypeResponse>? GetProductTypeByName(string? name)
        {
            _logger.LogInformation("GetTypeByName service executed");
            _logger.LogDebug($"- {name} - is the key for search in type database");
            if (name == null)
            {
                _logger.LogError($"name for type is null");
                throw new ArgumentNullException(nameof(name));
            }
            
            ProductType type = await _productTypeRepository.GetProductTypeByName(name);
            _diagnostic.Set("GetTypeByName", type);
            return type.ToProductTypeResponse();
        }
    }
};