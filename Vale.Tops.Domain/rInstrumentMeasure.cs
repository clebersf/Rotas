namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rInstrumentMeasure")]
    public partial class rInstrumentMeasure : Entity
    {
        
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual string Value { get; set; }
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorString { get; set; }
        public virtual DateTime dh { get; set; }
        public virtual DateTime LastDh { get; set; }
        public virtual bool isUpdating { get; set; }
        public virtual bool Write { get; set; }
        public virtual string LastValue { get; set; }
        public virtual Location Location { get; set; }
        public virtual long TagId { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
