using System;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO.ProductTypeDTO
{
    /// <summary>
    /// DTO Class for Adding a new Product Type
    /// </summary>
    public class ProductTypeAddRequest
    {
        [Required]
        public string? TypeNameEN { get; set; }

        [Required]
        public string? TypeNameFr { get; set; }

        public ProductType ToProductType()
        {
            return new ProductType()
            {
                TypeId = Guid.NewGuid(),
                TypeNameEN = TypeNameEN,
                TypeNameFr = TypeNameFr
            };
        }
    }
}
