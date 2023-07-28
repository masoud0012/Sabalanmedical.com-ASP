using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductDescriptionDTO
{
    public class ProductDescResponse
    {
        public Guid DesctiptionID { get; set; }
        public Guid ProductID { get; set; }
        public string? DescTitle { get; set; }
        public string? Description { get; set; }
        public ProductDescUpdateRequest ToProductDescUpdateRequest()
        {
            return new ProductDescUpdateRequest()
            {
                DesctiptionID = DesctiptionID,
                ProductID = ProductID,
                DescTitle = DescTitle,
                Description = Description
            };
        }
    }

    public static class ProductDescExtension
    {
        public static ProductDescResponse ToProductDescResponse(this ProductDesc productDesc)
        {
            return new ProductDescResponse()
            {
                DesctiptionID = productDesc.DesctiptionID,
                ProductID = productDesc.ProductID,
                DescTitle = productDesc.DescTitle,
                Description = productDesc.Description
            };
        }
    }

}
