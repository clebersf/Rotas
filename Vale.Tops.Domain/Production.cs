namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production")]
    public partial class Production : Entity
    {
        public Production ()
        {
            rProductionOrigin = new HashSet<rProductionOrigin>();
        }
        [Key]
        public virtual Guid Id { get; set; }

        
        public DateTime? dhi { get; set; }

        public DateTime? dhf { get; set; }

        public rProductionStock rProductionStock { get; set; }

        public rProductionBoarding rProductionBoarding { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rProductionOrigin> rProductionOrigin { get; set; }

        public bool Final { get; set; }

        public bool Active { get; set; }
    }
}
