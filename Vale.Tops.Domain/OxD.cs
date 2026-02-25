namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OxD")]
    public partial class OxD : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OxD()
        {
            rRouteOxD = new HashSet<rRouteOxD>();
            
        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual Location Location { get; set; }
        
        public long? OriginId { get; set; }
        public virtual Location Origin { get; set; }

        public long? DestinationId { get; set; }
        public virtual Location Destination { get; set; }
        


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rRouteOxD> rRouteOxD { get; set; }
    }
}
