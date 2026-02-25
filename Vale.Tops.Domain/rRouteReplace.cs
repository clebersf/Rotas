namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRouteReplace")]
    public partial class rRouteReplace : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rRouteReplace()
        {

        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual long LocationId { get; set; }
        public virtual Location Location { get; set; }
        public virtual DateTime dh { get; set; }
    }
}
