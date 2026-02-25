namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Plc")]
    public partial class Plc : Entity
    {
        public Plc ()
        {
            Tag = new HashSet<Tag>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public string Resource { get; set; }
        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tag { get; set; }
    }
}
