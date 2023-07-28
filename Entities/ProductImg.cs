using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ProductImg
    {
        [Key]
        public Guid ImageID { get; set; }

        [ForeignKey(nameof(ImageID))]
        public Guid ProductID { get; set; }
        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }
}
