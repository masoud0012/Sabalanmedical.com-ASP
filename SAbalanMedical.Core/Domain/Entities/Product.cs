using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Product : BaseEntity
    {
        public Guid TypeId { get; set; }
        [StringLength(100)]
        public string? ProductNameEn { get; set; }
        [StringLength(200)]
        public string? ProductNameFr { get; set; }

        [StringLength(200)]
        public string? ProductUrl { get; set; }
        public bool isHotSale { get; set; } = false;

        public bool isManufactured { get; set; } = false;

        [ForeignKey("TypeId")]
        public ProductType? ProductType { get; set; }

        public virtual ICollection<ProductImg>? ProductImages { get; set; }
        public virtual ICollection<ProductProperty>? ProductProperties { get; set; }
        public virtual ICollection<ProductDesc>? ProductDescriptions { get; set; }
        public override string ToString()
        {
            return $"ProductName={ProductNameEn}" +
                $"\tProdctNameFr={ProductNameFr}\tisMAnufactured={isManufactured}\tisHotsale={isHotSale}";
        }
    }
}
