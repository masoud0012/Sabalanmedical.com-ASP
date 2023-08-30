using Azure;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.DTO.ProductTypeDTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;
using System.Text;
using System.IO;
using System.Reflection;

namespace Services
{
    public class ProductService : IProductService

    {
        private readonly SabalanDbContext _sabalanDbContext;
        //Constractor
        public ProductService(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }
        private ProductResponse ConvertToProductResponse(Product product)
        {
            ProductResponse productResponse = product.ToProductResponse();
            productResponse.TypeNameEN = _sabalanDbContext.ProductTypes.FirstOrDefault(t =>
            t.TypeId == product.TypeId)?.TypeNameEN;
            productResponse.TypeNameFr = _sabalanDbContext.ProductTypes.FirstOrDefault(t =>
            t.TypeId == product.TypeId)?.TypeNameFr;
            productResponse.ImageUrl = _sabalanDbContext.ProductImgs.FirstOrDefault(t =>
            t.ProductID == product.ProductID)?.ImageUrl;
            return productResponse;
        }
        public async Task<ProductResponse> AddProduct(ProductAddRequest? productAddRequest)
        {
            if (productAddRequest is null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }

            ValidationHelper.ModelValidation(productAddRequest);
            if (_sabalanDbContext.Products.Count(t => t.ProductNameEn == productAddRequest.ProductNameEn
            || t.ProductNameFr == productAddRequest.ProductNameFr) > 0)
            {
                throw new ArgumentException("Product Name is duplicated");
            }
            Product product = productAddRequest.ToProduct();
            // _sabalanDbContext.sp_AddProduct(product);
            _sabalanDbContext.Add(product);
            await _sabalanDbContext.SaveChangesAsync();
            return ConvertToProductResponse(product);
        }
        public async Task<List<ProductResponse>> GetAllProducts()
        {
            List<Product> products = await _sabalanDbContext.Products.Include("ProductType")
                .Include("ProductImg")
                .Include("ProductDesc")
                .Include("ProductProperty")
                .ToListAsync();
            /*List<ProductResponse> productResponses = new List<ProductResponse>();
            foreach (var item in products)
            {
                productResponses.Add(ConvertToProductResponse(item));
            }*/
            List<ProductResponse> productResponses= products.Select(t => t.ToProductResponse()).ToList();
            return productResponses;
        }
        public async Task<ProductResponse>? GetProductById(Guid? productID)
        {
            if (productID == null)
            {
                return null;
            }
            //ProductResponse? product= _products.FirstOrDefault(temp => temp.ProductID == guid)?.ToProductResponse();
            // Product? product = _sabalanDbContext.Products.FirstOrDefault(t => t.ProductID == productID);
            Product? product = _sabalanDbContext.sp_GetProductById(productID ?? Guid.Empty);
            if (product == null)
            {
                return null;
            }
            else
            {
                return ConvertToProductResponse(product);
            }
        }
        public async Task<List<ProductResponse>>? GetFilteredProduct(string searchBy, string? searchKey)
        {
            List<ProductResponse> allProducts = await GetAllProducts();
            List<ProductResponse> matcheProductes = allProducts;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchKey))
                return allProducts;

            switch (searchBy)
            {
                case nameof(ProductResponse.TypeId):
                    matcheProductes = allProducts.Where(t => t.TypeId == t.TypeId).ToList();
                    break;
                case nameof(ProductResponse.TypeNameEN):
                    matcheProductes = allProducts.Where(item => item.TypeNameEN == searchKey).ToList();
                    break;
                case nameof(ProductResponse.TypeNameFr):
                    matcheProductes = allProducts.Where(item => item.TypeNameFr == searchKey).ToList();
                    break;
                case nameof(ProductResponse.ProductNameEn):
                    matcheProductes = allProducts.Where(item =>
                    !string.IsNullOrEmpty(item.ProductNameEn) ?
                    item.ProductNameEn.Contains(searchKey, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(ProductResponse.ProductNameFr):
                    matcheProductes = allProducts.Where(item =>
                    !string.IsNullOrEmpty(item.ProductNameFr) ?
                    item.ProductNameFr.Contains(searchKey, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(ProductResponse.isHotSale):
                    matcheProductes = allProducts.Where(item =>
                    (item.isHotSale != null) ? item.isHotSale.Equals(searchKey) : true).ToList();
                    break;
                case (nameof(ProductResponse.isManufactured)):
                    matcheProductes = allProducts.Where(item =>
                    item.isManufactured != null ? item.isManufactured.Equals(searchKey) : true).ToList();
                    break;
            }

            return matcheProductes;
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
            Product? matchedProduct = _sabalanDbContext.sp_GetProductById(productUpdateRequest.ProductID);
            if (matchedProduct == null)
            {
                throw new ArgumentException("No Product was found in the list");
            }
            _sabalanDbContext.sp_UpdateProduct(productUpdateRequest.ToProduct());
            /*            matchedProduct.TypeId = productUpdateRequest.TypeId;
                        matchedProduct.ProductNameEn = productUpdateRequest.ProductNameEn;
                        matchedProduct.ProductNameFr = productUpdateRequest.ProductNameFr;
                        matchedProduct.isHotSale = productUpdateRequest.isHotSale;
                        matchedProduct.isManufactured = productUpdateRequest.isManufactured;
                        matchedProduct.ProductUrl = productUpdateRequest.ProductUrl;*/
            await _sabalanDbContext.SaveChangesAsync();
            return ConvertToProductResponse(matchedProduct);
        }
        public async Task<bool> DeleteProduct(Guid? productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException("ProductId can not be null");
            }
            Product? mathedProduct = _sabalanDbContext.Products.FirstOrDefault(t => t.ProductID == productId);
            if (mathedProduct == null)
            {
                return false;
            }
            _sabalanDbContext.Products.Remove(mathedProduct);
            await _sabalanDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductResponse>? GetProductByProductUrl(string? productUrl)
        {
            Product? product = await _sabalanDbContext.Products.FirstOrDefaultAsync(t => t.ProductUrl == productUrl);
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
            List<Product> products = await _sabalanDbContext.sp_GetAllProducts();
            List<ProductResponse> productResponses = products.Select(t => ConvertToProductResponse(t)).ToList();
            foreach (var item in productResponses)
            {
                csvWriter.WriteField(item.TypeNameEN);
                csvWriter.WriteField(item.TypeNameFr);
                csvWriter.WriteField(item.ProductNameEn);
                csvWriter.WriteField(item.ProductNameFr);
                csvWriter.WriteField(item.isHotSale == false ? "No" : "Yes");
                csvWriter.WriteField(item.ProductUrl);
                csvWriter.WriteField(item.isManufactured);
                csvWriter.WriteField(item.ImageUrl);
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
