using Entities;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.DTO.ProductTypeDTO;
using ServiceContracts.Enums;
using Services.Helpers;


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
        private  ProductResponse ConvertToProductResponse(Product product)
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
        public ProductResponse AddProduct(ProductAddRequest? productAddRequest)
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
            _sabalanDbContext.Products.Add(product);
            _sabalanDbContext.SaveChanges();
            //_products.Add(product);
            return ConvertToProductResponse(product);
        }
        public List<ProductResponse> GetAllProducts()
        {
            List<ProductResponse> products = _sabalanDbContext.Products.ToList()
                .Select(t =>ConvertToProductResponse(t)).ToList();

            return products;
        }
        public ProductResponse? GetProductById(Guid? productID)
        {
            if (productID == null)
            {
                return null;
            }
            //ProductResponse? product= _products.FirstOrDefault(temp => temp.ProductID == guid)?.ToProductResponse();
            Product? product = _sabalanDbContext.Products.FirstOrDefault(t => t.ProductID == productID);
            if (product == null)
            {
                return null;
            }
            else
            {
                return ConvertToProductResponse(product);
            }
        }
        public List<ProductResponse>? GetFilteredProduct(string searchBy, string? searchKey)
        {
            List<ProductResponse> allProducts = GetAllProducts();
            List<ProductResponse> machteProductes = allProducts;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchKey))
                return allProducts;

            switch (searchBy)
            {
                case nameof(ProductResponse.TypeNameEN):
                    machteProductes = allProducts.Where(item => item.TypeNameEN == searchKey).ToList();
                    break;
                case nameof(ProductResponse.TypeNameFr):
                    machteProductes = allProducts.Where(item => item.TypeNameFr == searchKey).ToList();
                    break;
                case nameof(ProductResponse.ProductNameEn):
                    machteProductes = allProducts.Where(item =>
                    !string.IsNullOrEmpty(item.ProductNameEn) ?
                    item.ProductNameEn.Contains(searchKey, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(ProductResponse.ProductNameFr):
                    machteProductes = allProducts.Where(item =>
                    !string.IsNullOrEmpty(item.ProductNameFr) ?
                    item.ProductNameFr.Contains(searchKey, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(ProductResponse.isHotSale):
                    machteProductes = allProducts.Where(item =>
                    (item.isHotSale != null) ? item.isHotSale.Equals(searchKey) : true).ToList();
                    break;
                case (nameof(ProductResponse.isManufactured)):
                    machteProductes = allProducts.Where(item =>
                    item.isManufactured != null ? item.isManufactured.Equals(searchKey) : true).ToList();
                    break;
            }

            return machteProductes;
        }

        public List<ProductResponse> GetSortedProducts(List<ProductResponse> allProducts, string SortBy, SortOrderOptions sortOrder)
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
        public ProductResponse UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(productUpdateRequest));
            }
            ValidationHelper.ModelValidation(productUpdateRequest);
            Product? matchedProduct = _sabalanDbContext.Products.FirstOrDefault(t => t.ProductID == productUpdateRequest.ProductID);
            if (matchedProduct == null)
            {
                throw new ArgumentException("No Product was found in the list");
            }
            matchedProduct.TypeId = productUpdateRequest.TypeId;
            matchedProduct.ProductNameEn = productUpdateRequest.ProductNameEn;
            matchedProduct.ProductNameFr = productUpdateRequest.ProductNameFr;
            matchedProduct.isHotSale = productUpdateRequest.isHotSale;
            matchedProduct.isManufactured = productUpdateRequest.isManufactured;
            matchedProduct.ProductUrl = productUpdateRequest.ProductUrl;
            _sabalanDbContext.SaveChanges();
            return ConvertToProductResponse(matchedProduct);
        }
        public bool DeleteProduct(Guid? productId)
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
            _sabalanDbContext.SaveChanges();
            return true;
        }

        public ProductResponse? GetProductByProductUrl(string? productUrl)
        {
          return  _sabalanDbContext.Products.FirstOrDefault(t => t.ProductUrl == productUrl).ToProductResponse();
        }
    }
}
