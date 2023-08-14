using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }

        public Guid TypeId { get; set; }
        [StringLength(100)]
        public string? ProductNameEn { get; set; }
        [StringLength(200)]
        public string? ProductNameFr { get; set; }

        [StringLength(200)]
        public string? ProductUrl { get; set; }
        public bool isHotSale { get; set; }=false;

        public bool isManufactured { get; set; } = false;

        [ForeignKey("TypeId")]
        public ProductType? ProductType { get; set; }
    }
}
