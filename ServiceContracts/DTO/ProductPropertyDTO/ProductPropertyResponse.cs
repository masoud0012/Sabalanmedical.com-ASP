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
        public Guid propertyID { get; set; }
        public Guid ProductID { set; get; }
        public string? PropertyTitle { get; set; }
        public string? PropertyDetail { get; set; }
        public ProductPropertyUpdateRequest ToProductPropertyUpdateRequest()
        {
            return new ProductPropertyUpdateRequest()
            {
                propertyID = propertyID,
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
                propertyID = productProperty.propertyID,
                ProductID = productProperty.propertyID,
                PropertyTitle = productProperty.PropertyTitle,
                PropertyDetail = productProperty.PropertyDetail,
            };
        }
    }
}
