namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRouteGraphGpv")]
    public partial class rRouteGraphGpv : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rRouteGraphGpv()
        {

        }
        [Key]
        [ForeignKey("RouteGraph")]
        public virtual long Id { get; set; }

        public virtual RouteGraph RouteGraph { get; set; }

        public virtual long GpvCode { get; set; }


    }
}
