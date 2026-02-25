namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rConveyorLenght")]
    public partial class rConveyorLenght : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rConveyorLenght()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual double Lenght { get; set; }

        public virtual Conveyor Conveyor { get; set; }
    }
}
