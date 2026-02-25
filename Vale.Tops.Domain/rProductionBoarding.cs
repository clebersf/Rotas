namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rProductionBoarding")]
    public partial class rProductionBoarding : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rProductionBoarding()
        {

        }
        [Key]
        public virtual Guid Id { get; set; }
        public virtual long BerthId { get; set; }
        public virtual Location Berth { get; set; }
        public virtual int Compartment { get; set; }
        public virtual double Load { get; set; }
        public virtual double Current { get; set; }
        public virtual double InitialLoad { get; set; }
        public Production Production { get; set; }

        public Instrument Instrument { get; set; }
        public virtual long InstrumentId { get; set; }
    }
}
