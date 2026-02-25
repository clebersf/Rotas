namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRouteTravel")]
    public partial class rRouteTravel : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rRouteTravel()
        {

        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }

        public virtual Location Location { get; set; }

        public virtual double Constant { get; set; }

        public virtual double Variable { get; set; }

        public virtual double GoalI { get; set; }

        public virtual double GoalF { get; set; }

    }
}
