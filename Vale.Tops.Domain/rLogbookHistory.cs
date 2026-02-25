namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLogbookHistory")]
    public partial class rLogbookHistory : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rLogbookHistory()
        {
            
        }
        [Key]
        public virtual Guid Id { get; set; }
        public Guid LogbookId { get; set; }

        public Guid HistoryId { get; set; }

        public virtual History History { get; set; }

        public virtual Logbook Logbook { get; set; }

    }
}
