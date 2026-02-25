namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Conveyor")]
    public partial class Conveyor : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Conveyor()
        {
            
        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual Location Location { get; set; }

        public virtual rConveyorLenght rConveyorLenght { get; set; }
        public virtual rConveyorConsumption rConveyorConsumption { get; set; }
        public virtual rConveyorSpeed rConveyorSpeed { get; set; }
        public virtual rTagConveyorClean rTagConveyorClean { get; set; }

        public virtual rConveyorCapacity rConveyorCapacity { get; set; }

        
        
    }
}
