using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductPropertyDTO
{
    public class ProductPropertyResponse
    {
        public Guid Id { get; set; }
        public Guid ProductID { set; get; }
        public string? PropertyTitle { get; set; }
        public string? PropertyDetail { get; set; }
        public override string ToString()
        {
            return $"PropertID={Id},\t productId={ProductID},\t PRoductTitle={PropertyTitle},\t PropertyDetails={PropertyDetail}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType()!=typeof(ProductPropertyResponse)) return false;
            ProductPropertyResponse property = (ProductPropertyResponse)obj;
            return property.Id == Id && property.ProductID == ProductID && property.PropertyTitle == PropertyTitle && property.PropertyDetail == PropertyDetail;
        }

        public ProductPropertyUpdateRequest ToProductPropertyUpdateRequest()
        {
            return new ProductPropertyUpdateRequest()
            {
                Id = Id,
                ProductID = ProductID,
                PropertyTitle = PropertyTitle,
                PropertyDetail = PropertyDetail
            };
        }
    }
    public static class ProductPropertyExtension
    {
        public static ProductPropertyResponse ToProductPropertyResponse(this ProductProperty productProperty)
        {
            return new ProductPropertyResponse()
            {
                Id = productProperty.Id,
                ProductID = productProperty.ProductID,
                PropertyTitle = productProperty.PropertyTitle,
                PropertyDetail = productProperty.PropertyDetail,
            };
        }
    }
}
