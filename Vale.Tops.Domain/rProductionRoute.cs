namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rProductionRoute")]
    public partial class rProductionRoute : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rProductionRoute()
        {

        }
        [Key]
        public virtual Guid Id { get; set; }
        public virtual Guid ProductionId { get; set; }
        public virtual long RouteId { get; set; }
        public Location Route { get; set; }
        public virtual Production Production { get; set; }
        public virtual DateTime? dhi { get; set; }
        public virtual DateTime? dhf { get; set; }
        public virtual double Load { get; set; }
        public virtual double InitialLoad { get; set; }
        public virtual bool Final { get; set; }
        public virtual bool Active { get; set; }
        public virtual int NWagon { get; set; }
        public virtual int Dismember { get; set; }

        public virtual int? InitialNWagon { get; set; }
        public virtual long? InstrumentId { get; set; }
        public virtual Instrument Instrument { get; set; }

        public virtual bool Cleaning { get; set; }

        public virtual double? FinalLoad { get; set; }

        public virtual string Gate { get; set; }
    }
}
