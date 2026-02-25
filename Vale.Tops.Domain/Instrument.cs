namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Instrument")]
    public partial class Instrument : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instrument()
        {
            rProductionRoutes = new HashSet<rProductionRoute>();
            rProductionStocks = new HashSet<rProductionStock>();
            rProductionBoardings = new HashSet<rProductionBoarding>();
        }
        [Key]
        [ForeignKey("Location")]
        public virtual long Id { get; set; }
        public virtual Location Location { get; set; }

        //Relacionamentos 1:N

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rProductionRoute> rProductionRoutes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rProductionStock> rProductionStocks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rProductionBoarding> rProductionBoardings { get; set; }

    }
}
