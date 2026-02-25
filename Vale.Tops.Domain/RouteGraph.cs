namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RouteGraph")]
    public partial class RouteGraph : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RouteGraph()
        {
            rRouteGraphSequence = new HashSet<rRouteGraphSequence>();
        }
        [Key]
        public virtual long Id { get; set; }

        public virtual string Route { get; set; }

        public virtual string Summary { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rRouteGraphSequence> rRouteGraphSequence { get; set; }

    }
}
