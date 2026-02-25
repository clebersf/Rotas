namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tag")]
    public partial class Tag : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tag()
        {
            rInstrumentMeasure = new HashSet<rInstrumentMeasure>();
            rTagGroup = new HashSet<rTagGroup>();
        }

        [Key]
        public virtual long Id { get; set; }
        public virtual long PlcId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Plc Plc { get; set; }
        public virtual rTagInformation rTagInformation { get; set; }
        public virtual rTagWrite rTagWrite { get; set; }
        public virtual rTagConveyorClean rTagConveyorClean { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rInstrumentMeasure> rInstrumentMeasure { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rTagGroup> rTagGroup { get; set; }

    }
}
