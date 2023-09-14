using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;
using System.Text;
using System.IO;
using System.Reflection;
using RepositoryContracts;


namespace Services
{
    public class ProductService : IProductService

    {
        private readonly IProductRepository _productRepository;
        //Constractor
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductResponse> AddProduct(ProductAddRequest? productAddRequest)
        {
            if (productAddRequest is null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }
            ValidationHelper.ModelValidation(productAddRequest);
            if (await _productRepository.GetProductByName(productAddRequest.ProductNameEn, productAddRequest.ProductNameFr) != null)
            {
                throw new ArgumentException("Product Name is duplicated");
            }
            Product product = productAddRequest.ToProduct();
            // _sabalanDbContext.sp_AddProduct(product);
            await _productRepository.AddProduct(product);
            return product.ToProductResponse();
        }
        public async Task<List<ProductResponse>> GetAllProducts()
        {
            List<Product> products = await _productRepository.GetAllProducts();
            return products.Select(t => t.ToProductResponse()).ToList();
        }
        public async Task<ProductResponse>? GetProductById(Guid? productID)
        {
            if (productID == null)
            {
                throw new ArgumentException(nameof(productID));
            }
            Product? product = await _productRepository.GetProductById(productID.Value);
            // Product? product = _sabalanDbContext.sp_GetProductById(productID ?? Guid.Empty);
            if (product == null)
            {
                return null;
            }
            return product.ToProductResponse();
        }
        public async Task<List<ProductResponse>>? GetFilteredProduct(string searchBy, string searchKey="c")
        {
            searchKey= searchKey ?? "";
            List<Product>? products = searchBy switch
            {
                nameof(ProductResponse.ProductNameEn) =>
                await _productRepository.GetFilteredProduct(t => t.ProductNameEn.Contains(searchKey)),
                nameof(ProductResponse.ProductNameFr) => await _productRepository.GetFilteredProduct(t =>
                t.ProductNameFr.Contains(searchKey)),
                nameof(ProductResponse.ProductUrl) => await _productRepository.GetFilteredProduct(t =>
               t.ProductUrl.Contains(searchKey)),
                nameof(Product.ProductType.TypeNameEN) => await _productRepository.GetFilteredProduct(t =>
                t.ProductType.TypeNameEN.Contains(searchKey)),
                nameof(Product.ProductType.TypeNameFr) => await _productRepository.GetFilteredProduct(t =>
               t.ProductType.TypeNameFr.Contains(searchKey)),
                nameof(ProductResponse.isManufactured) => await _productRepository.GetFilteredProduct(t =>
                t.isManufactured.ToString() == searchKey),
                nameof(ProductResponse.isHotSale) => await _productRepository.GetFilteredProduct(t =>
               t.isHotSale.ToString() == searchKey),

                _ => await _productRepository.GetAllProducts()
            };
            return products.Select(t => t.ToProductResponse()).ToList();
        }
        public async Task<List<ProductResponse>>? GetFilteredProduct(Guid typeId, string searchBy, string searchKey="")
        {

            if (typeId==null&&string.IsNullOrEmpty(searchKey)&&string.IsNullOrEmpty(searchKey))
            {
                return (await _productRepository.GetAllProducts()).Select(t => t.ToProductResponse()).ToList();
            }
           searchKey = searchKey ?? "";
            List<Product>? filteredProducts = searchBy switch
            {
                nameof(Product.ProductNameEn) => await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)),
                nameof(Product.ProductNameFr) => await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)),
                nameof(Product.ProductUrl) => await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)),
                nameof(Product.ProductType.TypeNameEN) => await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)),
                nameof(Product.ProductType.TypeNameFr) => await _productRepository.GetFilteredProduct(t => t.TypeId == typeId && t.ProductNameEn.Contains(searchKey)),

                _ => await _productRepository.GetFilteredProduct(t => t.TypeId == typeId)
            }; ;
            return filteredProducts.Select(t => t.ToProductResponse()).ToList();
        }

        public async Task<List<ProductResponse>> GetSortedProducts(List<ProductResponse> allProducts, string SortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(SortBy))
                return (allProducts);
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

            return SortedProducts;
        }
        public async Task<ProductResponse> UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(productUpdateRequest));
            }
            ValidationHelper.ModelValidation(productUpdateRequest);
            Product? matchedProduct = await _productRepository.GetProductById(productUpdateRequest.ProductID);
            if (matchedProduct == null)
            {
                throw new ArgumentException("No Product was found in the list");
            }

            //_sabalanDbContext.sp_UpdateProduct(productUpdateRequest.ToProduct());

            await _productRepository.UpdateProduct(productUpdateRequest.ToProduct());
            return matchedProduct.ToProductResponse();
        }
        public async Task<bool> DeleteProduct(Guid? productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException("ProductId can not be null");
            }
            Product? mathedProduct = await _productRepository.GetProductById(productId ?? Guid.Empty);
            if (mathedProduct == null)
            {
                return false;
            }
            return await _productRepository.DeleteProduct(mathedProduct);
        }

        public async Task<ProductResponse>? GetProductByProductUrl(string? productUrl)
        {
            if (productUrl == null)
            {
                throw new ArgumentNullException(nameof(productUrl));
            }
            Product? product = await _productRepository.GetProductByProductUrl(productUrl);
            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }
            return product.ToProductResponse();
        }

        public async Task<MemoryStream> ProductToCsv()
        {
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
            List<Product> products = await _productRepository.GetAllProducts();
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
