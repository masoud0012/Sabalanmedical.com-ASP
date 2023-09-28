using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductImageDTO
{
    public class ProductImageUpdateRequest
    {
        [Required(ErrorMessage = "{0} can not be null or blank")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "{0} can not be null or blank")]
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = "{0} can not be null or blank")]
        public string? ImageUrl { get; set; }

        public ProductImg ToProductImage()
        {
            return new ProductImg()
            {
                Id = Id,
                ProductID = ProductID,
                ImageUrl = ImageUrl
            };
        }
    }
}
