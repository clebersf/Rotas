namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product : Entity
    {
        public Product ()
        {
            
        }
        [Key]
        public virtual long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public float Density { get; set; }


        public string Family { get; set; }

        public DateTime Update { get; set; }
    }
}
