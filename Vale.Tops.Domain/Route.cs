namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Route")]
    public partial class Route : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Route()
        {
            //rProductionRoute = new HashSet<rProductionRoute>();
            //rRouteConveyor = new HashSet<rRouteConveyor>();
            
        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual long GpvCode { get; set; }
        public virtual bool Inactive { get; set; }
        public virtual Location Location { get; set; }
    }
}
