namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rConveyorConsumption")]
    public partial class rConveyorConsumption : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rConveyorConsumption()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual double Consumption { get; set; }

        public virtual Conveyor Conveyor { get; set; }
    }
}
