namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLocationValue")]
    public partial class rLocationValue : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public float? ValueFloat { get; set; }
        public string ValueString { get; set; }
        public bool? ValueBool { get; set; }
        public DateTime ValueDateTime { get; set; }
        public virtual Location Location { get; set; }

    }
}
