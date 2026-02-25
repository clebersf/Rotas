namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rConveyorCapacity")]
    public partial class rConveyorCapacity : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rConveyorCapacity()
        {

        }
        [Key]
        public virtual long Id { get; set; }



        public virtual double RollerTilt { get; set; }
        public virtual double Width { get; set; }

        public virtual double ConveyorIncline { get; set; }

        public virtual double Capacity { get; set; } // capacidade em ton/h

        public virtual Conveyor Conveyor { get; set; }
    }
}
