using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductImageDTO
{
    public class ProductImageAddRequest
    {
        [Required(ErrorMessage ="{0} can not be blank")]
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = "{0} can not be blank")]
        public string? ImageUrl { get; set; }

        public ProductImg ToProductImage()
        {
            return new ProductImg()
            {
                ImageID = Guid.NewGuid(),
                ProductID = ProductID,
                ImageUrl = ImageUrl
            };
        }
    }
}
