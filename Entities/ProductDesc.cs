using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ProductDesc
    {
        [Key]
        public Guid DesctiptionID { get; set; }

        public Guid ProductID { get; set; }
        [StringLength(200)]
        public string? DescTitle { get; set; } 

        public string? Description { get; set; }

       // public virtual ICollection<Product>? Products { get; set; }


    }
}
