namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rConveyorInputPosition")]
    public partial class rConveyorInputPosition : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rConveyorInputPosition()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual long? LocationAId { get; set; }

        public virtual double Position { get; set; }

        public virtual long? LocationBId { get; set; }

        public virtual Location LocationA { get; set; }
        public virtual Location LocationB { get; set; }
    }
}
