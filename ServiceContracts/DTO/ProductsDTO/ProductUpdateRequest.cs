using Entities;
using System;

using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO.ProductsDTO
{
    public class ProductUpdateRequest
    {
        [Required(ErrorMessage = "ProductShould have an ID")]
        public Guid ProductID { get; set; }
        [Required(ErrorMessage = "Product Type can not be empty")]
        public Guid TypeId { get; set; }

        [Required(ErrorMessage = "Product Name can not be empty")]
        public string? ProductNameEn { get; set; }

        [Required(ErrorMessage = "Product Name can not be empty")]
        public string? ProductNameFr { get; set; }

        public bool isHotSale { get; set; } = false;
        public string? ProductUrl { get; set; } = string.Empty;
        public bool isManufactured { get; set; } = false;

        public Product ToProduct()
        {
            return new Product()
            {
                Id = ProductID,
                TypeId = TypeId,
                ProductNameEn = ProductNameEn,
                ProductNameFr = ProductNameFr,
                ProductUrl = ProductUrl,
                isHotSale = isHotSale,
                isManufactured = isManufactured
            };
        }
    }
}