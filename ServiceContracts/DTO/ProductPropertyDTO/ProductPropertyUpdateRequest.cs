using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductPropertyDTO
{
    public class ProductPropertyUpdateRequest
    {
        [Required(ErrorMessage = "id can not be blank")]
        public Guid propertyID { get; set; }
        [Required(ErrorMessage = "Product Id can not be blank")]
        public Guid ProductID { set; get; }

        public string? PropertyTitle { get; set; }

        [Required(ErrorMessage = "{0} can not be blank")]
        public string? PropertyDetail { get; set; }
        public ProductProperty ToProductProperty()
        {
            return new ProductProperty()
            {
                Id = propertyID,
                ProductID = ProductID,
                PropertyTitle = PropertyTitle,
                PropertyDetail = PropertyDetail
            };
        }
    }
}
