namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLocationEnable")]
    public partial class rLocationEnable : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public bool Enable { get; set; }
        public virtual Location Location { get; set; }

    }
}
