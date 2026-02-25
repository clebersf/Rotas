namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Boarding")]
    public partial class Boarding : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Boarding()
        {

        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual Location Location { get; set; }

    }
}
