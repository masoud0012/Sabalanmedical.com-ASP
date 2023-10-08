using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using IRepository;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
namespace Services
{
    public class ProductService : IProductService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IDiagnosticContext _diagnostic;//Enables the complition of Logs check HTTP GET/....
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork,
            ILogger<ProductService> logger, IDiagnosticContext diagnosticContext)
        {
            _logger = logger;
            _diagnostic = diagnosticContext;
            _productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<ProductResponse> AddProduct(ProductAddRequest? productAddRequest)
        {
            _logger.LogInformation("Start AddProduct");

            if (productAddRequest is null)
            {
                _logger.LogError($"{nameof(productAddRequest)} is null");
                throw new ArgumentNullException(nameof(productAddRequest));
            }

            ValidationHelper.ModelValidation(productAddRequest);
            if (await _productRepository.GetProductByName(productAddRequest.ProductNameEn, productAddRequest.ProductNameFr) != null)
            {
                _logger.LogError("Duplicate product add request!");
                throw new ArgumentException("Product Name is duplicated");
            }

            Product product = productAddRequest.ToProduct();
            await _productRepository.Add(product);
            await _unitOfWork.SaveChanges();

            _diagnostic.Set("AddedProduct", product.ToString());
            return product.ToProductResponse();
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was called");
            List<Product> products;
            using (Operation.Time("GetAllProducts from serilog timing"))
            {
                products = (await _productRepository.GetAllAsync()).Include("ProductType").Include("ProductImages")
                .Include("ProductDescriptions")
                .Include("ProductProperties").ToList();
            }
            _diagnostic.Set("Products", products);//Print all Products into the end of log
            return products.Select(t => t.ToProductResponse()).ToList();
        }
        public async Task<ProductResponse>? GetProductById(Guid? productID)
        {
            _logger.LogInformation("GetProductById executed");
            if (productID == null)
            {
                _logger.LogError("Id is null");
                throw new ArgumentNullException(nameof(productID));
            }
            Product? product = (await _productRepository.GetById(productID.Value))?.Include("ProductType")?.Include("ProductImages")?
                .Include("ProductDescriptions")?
                .Include("ProductProperties").SingleOrDefault();
            if (product == null)
            {
                _logger.LogError("No prodct was found");
                throw new ArgumentException(nameof(productID));
            }
            _diagnostic.Set("ProductById", product);
            return product.ToProductResponse();
        }
        public async Task<List<ProductResponse>>? GetProductByTypeName(string? type)
        {
            _logger.LogInformation("GetProductByTypeName executed");
            if (type is null)
            {
                _logger.LogError("type is null");
                throw new ArgumentNullException(nameof(type));
            }

            List<Product> productsByType = await _productRepository.GetProductsByTypeName(type);

            _diagnostic.Set("productsByType", productsByType);

            return productsByType.Select(t => t.ToProductResponse()).ToList();

        }
        public async Task<List<ProductResponse>>? GetProductByTypeId(Guid? typeId)
        {
            _logger.LogInformation("GetProductByTypeId executed");
            if (typeId is null || typeId == Guid.Empty)
            {
                _logger.LogError("Id is nll");
                throw new ArgumentNullException(nameof(typeId));
            }
            var productsByType = await _productRepository.GetProductsByTypeId(typeId.Value);
            _diagnostic.Set("GetProductByTypeId", productsByType);
            return productsByType.Select(t => t.ToProductResponse()).ToList();
        }
        public async Task<List<ProductResponse>> GetFilteredProduct(string searchBy, string searchKey)
        {
            _logger.LogInformation("GetFilteredProduct executed");
            List<Product>? products;
            using (Operation.Time("Time for get filtered products"))
            {
                products = searchBy switch
                {
                    nameof(ProductResponse.ProductNameEn) =>
                    (await _productRepository.GetFilteredProduct(t =>

                    t.ProductNameEn.Contains(searchKey)))?.ToList(),

                    nameof(ProductResponse.ProductNameFr) => (await _productRepository.GetFilteredProduct(t =>
                    t.ProductNameFr.Contains(searchKey)))?.ToList(),

                    nameof(ProductResponse.ProductUrl) => (await _productRepository.GetFilteredProduct(t =>
                   t.ProductUrl.Contains(searchKey)))?.ToList(),

                    nameof(Product.ProductType.TypeNameEN) => (await _productRepository.GetFilteredProduct(t =>
                    t.ProductType.TypeNameEN.Contains(searchKey)))?.ToList(),

                    nameof(Product.ProductType.TypeNameFr) => (await _productRepository.GetFilteredProduct(t =>
                   t.ProductType.TypeNameFr.Contains(searchKey)))?.ToList(),

                    nameof(ProductResponse.isManufactured) => (await _productRepository.GetFilteredProduct(t =>
                    t.isManufactured.ToString() == searchKey))?.ToList(),

                    nameof(ProductResponse.isHotSale) => (await _productRepository.GetFilteredProduct(t =>
                   t.isHotSale.ToString() == searchKey))?.ToList(),

                    _ => (await _productRepository.GetAllAsync()).ToList()
                };
            }
            _diagnostic.Set("FilteredProducts", products);
            return products.Select(t => t.ToProductResponse()).ToList();
        }
        public async Task<List<ProductResponse>>? GetFilteredProduct(Guid? typeId, string searchBy, string searchKey = "")
        {
            _logger.LogInformation("GetFilteredProduct executed");
            _logger.LogDebug($"typeId={typeId}\tsearchBy={searchBy}\tsearchKey={searchKey}");
            if (typeId == null && string.IsNullOrEmpty(searchKey) && string.IsNullOrEmpty(searchKey))
            {
                _logger.LogInformation("All inputs are null returns all Products");
                return (await _productRepository.GetAllAsync()).Select(t => t.ToProductResponse()).ToList();
            }
            List<Product>? filteredProducts;
            using (Operation.Time("Time fo FilteredProfucts with typeId"))
            {
                filteredProducts = searchBy switch
                {
                    nameof(Product.ProductNameEn) => (await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)))?.ToList(),
                    nameof(Product.ProductNameFr) => (await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)))?.ToList(),
                    nameof(Product.ProductUrl) => (await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)))?.ToList(),
                    nameof(Product.ProductType.TypeNameEN) => (await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)))?.ToList(),
                    nameof(Product.ProductType.TypeNameFr) => (await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)))?.ToList(),

                    _ => (await _productRepository.GetFilteredProduct(t => t.TypeId == typeId, 0, 50))?.ToList()
                };
            }
            _diagnostic.Set("filteredProducts", filteredProducts);
            return filteredProducts.Select(t => t.ToProductResponse()).ToList();
        }
        public async Task<List<ProductResponse>> GetSortedProducts(List<ProductResponse> allProducts, string SortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedProducts executed");
            if (string.IsNullOrEmpty(SortBy))
                return allProducts;
            List<ProductResponse> SortedProducts = (SortBy, sortOrder) switch
            {
                (nameof(ProductResponse.ProductNameEn), SortOrderOptions.Asc) => allProducts.OrderBy(t => t.ProductNameEn, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(ProductResponse.ProductNameEn), SortOrderOptions.Desc) => allProducts.OrderByDescending(t => t.ProductNameEn, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(ProductResponse.ProductNameFr), SortOrderOptions.Asc) => allProducts.OrderBy(t => t.ProductNameFr, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(ProductResponse.ProductNameFr), SortOrderOptions.Desc) => allProducts.OrderByDescending(t => t.ProductNameFr, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(ProductResponse.isManufactured), SortOrderOptions.Asc) => allProducts.OrderBy(t => t.ProductNameFr).ToList(),
                (nameof(ProductResponse.isManufactured), SortOrderOptions.Desc) => allProducts.OrderByDescending(t => t.ProductNameFr).ToList(),

                (nameof(ProductResponse.isHotSale), SortOrderOptions.Asc) => allProducts.OrderBy(t => t.ProductNameFr).ToList(),
                (nameof(ProductResponse.isHotSale), SortOrderOptions.Desc) => allProducts.OrderByDescending(t => t.ProductNameFr).ToList(),
                _ => allProducts
            };
            _diagnostic.Set("SortedProducts", SortedProducts);
            return SortedProducts;
        }
        public async Task<ProductResponse> UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            _logger.LogInformation("UpdateProduct executed");
            if (productUpdateRequest == null)
            {
                _logger.LogError("request is null");
                throw new ArgumentNullException(nameof(productUpdateRequest));
            }
            ValidationHelper.ModelValidation(productUpdateRequest);
            Product? matchedProduct = (await _productRepository.GetById(productUpdateRequest.Id)).SingleOrDefault();
            if (matchedProduct == null)
            {
                _logger.LogError("No matched product was found");
                throw new ArgumentException("No Product was found in the list");
            }
            matchedProduct.TypeId = productUpdateRequest.TypeId;
            matchedProduct.isManufactured = productUpdateRequest.isManufactured;
            matchedProduct.ProductNameFr = productUpdateRequest.ProductNameFr;
            matchedProduct.ProductNameEn = productUpdateRequest.ProductNameEn;
            matchedProduct.ProductUrl = productUpdateRequest.ProductUrl;
            matchedProduct.isHotSale = productUpdateRequest.isHotSale;
            await _productRepository.Update(matchedProduct);
            await _unitOfWork.SaveChanges();
            _diagnostic.Set("UpdateProduct", matchedProduct);
            return matchedProduct.ToProductResponse();
        }
        public async Task<bool> DeleteProduct(Guid? productId)
        {
            _logger.LogInformation("DeleteProduct executed");
            if (productId == null)
            {
                _logger.LogError("ProductId is null an exception");
                throw new ArgumentNullException("ProductId can not be null");
            }
            Product? mathedProduct = (await _productRepository.GetById(productId ?? Guid.Empty)).SingleOrDefault();
            if (mathedProduct == null)
            {
                _logger.LogError("No Product was found to be deleted");
                return false;
            }
            _logger.LogWarning($"try to delete {mathedProduct.ProductNameFr} from database!");
            await _productRepository.Delete(mathedProduct);
            await _unitOfWork.SaveChanges();
            _diagnostic.Set("DeleteProduct", mathedProduct);
            return true;

        }

        public async Task<ProductResponse>? GetProductByProductUrl(string? productUrl)
        {
            _logger.LogInformation("GetProductByProductUrl executed");
            if (productUrl == null)
            {
                _logger.LogError("url is null");
                throw new ArgumentNullException(nameof(productUrl));
            }
            Product? product = await _productRepository.GetProductByProductUrl(productUrl);
            if (product == null)
            {
                _logger.LogError("No Url was found");
                throw new ArgumentException(nameof(product));
            }
            _diagnostic.Set("GetProductByProductUrl", product);
            return product.ToProductResponse();
        }

        public async Task<MemoryStream> ProductToCsv()
        {
            _logger.LogInformation("ProductToCsv executed");
            MemoryStream memorySream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memorySream, Encoding.UTF8);
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
            foreach (var item in typeof(ProductResponse).GetProperties())
            {
                if (!item.Name.Contains("Id", StringComparison.OrdinalIgnoreCase))
                {
                    csvWriter.WriteField(item.Name);
                }
            }
            csvWriter.NextRecord();
            List<Product>? products = (await _productRepository.GetAllAsync(0, 50))?.ToList();
            List<ProductResponse> productResponses = products.Select(t => t.ToProductResponse()).ToList();
            foreach (var item in productResponses)
            {
                csvWriter.WriteField(item.productType.TypeNameEN);
                csvWriter.WriteField(item.productType.TypeNameFr);
                csvWriter.WriteField(item.ProductNameEn);
                csvWriter.WriteField(item.ProductNameFr);
                csvWriter.WriteField(item.isHotSale == false ? "No" : "Yes");
                csvWriter.WriteField(item.ProductUrl);
                csvWriter.WriteField(item.isManufactured);
                csvWriter.WriteField(item.productImgs?.FirstOrDefault());
                csvWriter.NextRecord();
                csvWriter.Flush();
            }
            memorySream.Position = 0;
            return memorySream;
        }

        public Task<MemoryStream> ProductToExcel()
        {
            throw new NotImplementedException();
        }


    }
}
