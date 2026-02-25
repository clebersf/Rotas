namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRouteGraphLocation")]
    public partial class rRouteGraphLocation : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rRouteGraphLocation()
        {

        }
        [Key]
        [ForeignKey("RouteGraph")]
        public virtual long Id { get; set; }
        public virtual RouteGraph RouteGraph { get; set; }

        public virtual long LocationId { get; set; }

        public virtual Location Location { get; set; }

    }
}
