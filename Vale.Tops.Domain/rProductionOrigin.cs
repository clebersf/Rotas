namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rProductionOrigin")]
    public partial class rProductionOrigin : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rProductionOrigin()
        {

        }
        [Key]
        public virtual Guid Id { get; set; }
        public virtual Guid ProductionId { get; set; }
        public virtual Production Production { get; set; }
        public virtual int GoalI { get; set; }
        public virtual int GoalF { get; set; }
    }
}
