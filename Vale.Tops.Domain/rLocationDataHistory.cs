namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLocationDataHistory")]
    public partial class rLocationDataHistory : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public virtual Guid Id { get; set; }

        public virtual long? LocationId { get; set; }

        public DateTime dh { get; set; }

        public double? Flow { get; set; }

        public string Material { get; set; }

        public string Operador { get; set; }

        public int? InitialMark { get; set; }

        public int? FinalMark { get; set; }

        public double? Total { get; set; }

        public int? OperationTime { get; set; }

        public virtual Location Location { get; set; }
    }
}
