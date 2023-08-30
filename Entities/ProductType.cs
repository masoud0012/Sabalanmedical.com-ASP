using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Domain Model For Types of Products
    /// </summary>
    public class ProductType
    {
        [Key]
        public Guid TypeId { get; set; }

        [StringLength(200)]
        public string? TypeNameEN { get; set; }

        [StringLength(400)]
        public string? TypeNameFr { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

    }
}