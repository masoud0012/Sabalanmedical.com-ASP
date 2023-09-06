using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    public class ProductProperty
    {
        [Key]
        public Guid propertyID { get; set; }

        public Guid ProductID { set; get; }
        [StringLength(100)]
        public string? PropertyTitle { get; set; }
        
        public string? PropertyDetail { get; set; }

        [ForeignKey("ProductID")]
        public Product? Product { get; set; }
    }
}
