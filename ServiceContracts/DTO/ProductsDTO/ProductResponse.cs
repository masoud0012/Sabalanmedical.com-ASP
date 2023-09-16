using Entities;
using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductPropertyDTO;
using System;
namespace ServiceContracts.DTO.ProductsDTO
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public Guid TypeId { get; set; }
        public string? ProductNameEn { get; set; }
        public string? ProductNameFr { get; set; }
        public bool isHotSale { get; set; }
        public string? ProductUrl { get; set; }
        public bool isManufactured { get; set; }
        public List<ProductImageResponse>? productImgs { get; set; }
        public List<ProductDescResponse>? productDescs { get; set; }
        public List<ProductPropertyResponse>? productProperties { get; set; }

        public ProductType? productType { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(ProductResponse))
            {
                return false;
            }
            ProductResponse response = (ProductResponse)obj;
            return response.ProductId == ProductId
                && response.TypeId == TypeId
                && response.ProductNameEn == ProductNameEn
                && response.ProductNameFr == ProductNameFr
                && response.isHotSale == isHotSale
                && response.ProductUrl == ProductUrl
                && response.isManufactured == isManufactured;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"TypeId:{TypeId} -- ProductName: {ProductNameEn} --/Farsi: {ProductNameFr}-- IsHotsale: {isHotSale} --ProductId:{ProductId}";
        }
        public ProductUpdateRequest ToProductUpdateRequest()
        {
            return new ProductUpdateRequest()
            {
                ProductID = ProductId,
                TypeId = TypeId,
                ProductNameEn = ProductNameEn,
                ProductNameFr = ProductNameFr,
                isManufactured = isManufactured,
                isHotSale = isHotSale,
                ProductUrl = ProductUrl
            };
        }
    }
    public static class ProductExtension
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                ProductId = product.Id,
                TypeId = product.TypeId,
                ProductNameEn = product.ProductNameEn,
                ProductNameFr = product.ProductNameFr,
                isHotSale = product.isHotSale,
                ProductUrl = product.ProductUrl,
                isManufactured = product.isManufactured,
                productType=product.ProductType,
                productDescs=product.ProductDescriptions?.Select(t=>t.ToProductDescResponse())?.ToList(),
                productImgs=product.ProductImages?.Select(t=>t.ToProductImgResponse()).ToList(),
                productProperties=product.ProductProperties?.Select(t=>t.ToProductPropertyResponse())?.ToList()
            };
        }
    }

}
