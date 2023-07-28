using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ProductProperty
    {
        [Key]
        public Guid propertyID { get; set; }
        [ForeignKey(nameof(propertyID))]
        public Guid ProductID { set; get; }
        [StringLength(100)]
        public string? PropertyTitle { get; set; }
        
        public string? PropertyDetail { get; set; }
    }
}
