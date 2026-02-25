namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRouteGraphSequence")]
    public partial class rRouteGraphSequence : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rRouteGraphSequence()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual int Order { get; set; }

        public virtual long LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual long RouteGraphId { get; set; }

        public virtual RouteGraph RouteGraph { get; set; }

    }
}
