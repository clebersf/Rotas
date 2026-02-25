namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rTagConveyorClean")]
    public partial class rTagConveyorClean : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rTagConveyorClean()
        {

        }
        [Key]
        [ForeignKey("Tag")]
        public virtual long Id { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual long ConveyorId { get; set; }

        public virtual Conveyor Conveyor { get; set; }
    }
}
