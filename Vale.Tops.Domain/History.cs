namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("History")]
    public partial class History : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public History()
        {
            rLogbookHistory = new HashSet<rLogbookHistory>();
            rLocationHistory = new HashSet<rLocationHistory>();
            
        }
        [Key]
        public virtual Guid Id { get; set; }
        public DateTime dh { get; set; }

        public string Action { get; set; }

        public string User { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rLogbookHistory> rLogbookHistory { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rLocationHistory> rLocationHistory { get; set; }
        
    }
}
