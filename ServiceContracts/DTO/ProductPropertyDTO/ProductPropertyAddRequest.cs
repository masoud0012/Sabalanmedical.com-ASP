using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ServiceContracts.DTO.ProductPropertyDTO
{
    public class ProductPropertyAddRequest
    {
        [Required(ErrorMessage ="Product Id can not be blank")]
        public Guid ProductID { set; get; }
        public string? PropertyTitle { get; set; }
        public string? PropertyDetail { get; set; }
        public ProductProperty ToProductProperty()
        {
            return new ProductProperty()
            {
                propertyID = Guid.NewGuid(),
                ProductID = this.ProductID,
                PropertyTitle = this.PropertyTitle,
                PropertyDetail = this.PropertyDetail
            };
        }
    }
}
