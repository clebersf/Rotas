namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Berth")]
    public partial class Berth : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Berth()
        {

        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual Location Location { get; set; }

    }
}
