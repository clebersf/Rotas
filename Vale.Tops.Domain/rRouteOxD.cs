namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRouteOxD")]
    public partial class rRouteOxD : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rRouteOxD()
        {

        }
        [Key]

        public virtual long Id { get; set; }

        public virtual long OxDId { get; set; }

        public virtual OxD OxD { get; set; }

        public virtual Location Location { get; set; }

        public virtual long LocationId { get; set; }

    }
}
