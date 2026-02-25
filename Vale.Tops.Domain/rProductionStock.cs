namespace Vale.Tops.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("rProductionStock")]
    public partial class rProductionStock : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rProductionStock()
        {

        }
        [Key]
        public virtual Guid Id { get; set; }
        public virtual int GoalI { get; set; }
        public virtual int GoalF { get; set; }
        public virtual double Load { get; set; }
        public virtual double? InitialLoad { get; set; }
        public virtual long? InstrumentId { get; set; }
        public virtual Instrument Instrument { get; set; }
        public Production Production { get; set; }
    }
}
