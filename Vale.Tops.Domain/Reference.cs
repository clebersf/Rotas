namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference")]
    public partial class Reference : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reference()
        {

        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual Location Location { get; set; }
        public long? LocReferenceId { get; set; }
        public virtual Location LocReference { get; set; }

        public virtual double Value { get; set; }
    }
}
