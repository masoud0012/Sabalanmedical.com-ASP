using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductImageDTO
{
    public class ProductImageResponse
    {
        public Guid Id { get; set; }
        public Guid ProductID { get; set; }
        //public Guid ProductTypeID { get; set; } 
        public string? ImageUrl { get; set; }
        public ProductImageUpdateRequest ToProductImgUpdateRequest()
        {
            return new ProductImageUpdateRequest()
            {
                Id = Id,
                ProductID = ProductID,
                ImageUrl = ImageUrl
            };
        }
    }

    public static class ProductImageExtension
    {
        public static ProductImageResponse ToProductImgResponse(this ProductImg productImg)
        {
            return new ProductImageResponse()
            {
                ProductID = productImg.ProductID,
                Id = productImg.Id,
                ImageUrl = productImg.ImageUrl,
            };
        }
    }
}
