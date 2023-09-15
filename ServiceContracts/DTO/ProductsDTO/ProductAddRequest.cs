using Entities;
using System;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO.ProductsDTO
{
    public class ProductAddRequest
    {
        [Required(ErrorMessage = "Product Name can not be empty")]       
        public Guid TypeId { get; set; }
        [Required(ErrorMessage = "Product Name can not be empty")]
        public string? ProductNameEn { get; set; }
        [Required(ErrorMessage = "Product Name can not be empty")]
        public string? ProductNameFr { get; set; }
        public bool isHotSale { get; set; } 
        public string? ProductUrl { get; set; }
        public bool isManufactured { get; set; } 
        public Product ToProduct()
        {
            return new Product()
            {
                ProductID = Guid.NewGuid(),
                TypeId = TypeId,
                ProductNameEn = ProductNameEn,
                ProductNameFr = ProductNameFr,
                ProductUrl = ProductNameEn?.Replace(" ","-"),
                isHotSale = isHotSale,
                isManufactured = isManufactured
            };
        }
    }

}
