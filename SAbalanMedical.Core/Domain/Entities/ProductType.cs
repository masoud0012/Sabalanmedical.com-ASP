using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Domain Model For Types of Products
    /// </summary>
    public class ProductType : BaseEntity
    {
        [StringLength(200)]
        public string? TypeNameEN { get; set; }

        [StringLength(400)]
        public string? TypeNameFr { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
        public override string ToString()
        {
            return $"{TypeNameEN},{TypeNameFr}";
        }
    }
}