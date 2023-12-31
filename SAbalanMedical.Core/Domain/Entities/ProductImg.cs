﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ProductImg : BaseEntity
    {
/*        [Key]
        public Guid ImageID { get; set; }*/

        public Guid ProductID { get; set; }
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [ForeignKey("ProductID")]
        public Product? Product { get; set; }

    }
}
