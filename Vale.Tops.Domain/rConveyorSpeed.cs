namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rConveyorSpeed")]
    public partial class rConveyorSpeed : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rConveyorSpeed()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual double Speed { get; set; }

        public virtual Conveyor Conveyor { get; set; }
    }
}
