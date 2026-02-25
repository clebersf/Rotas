namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Graph")]
    public partial class Graph : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Graph()
        {
            
        }
        [Key]
        public virtual long Id { get; set; }
        public long? LocationAId { get; set; }

        public long? LocationBId { get; set; }

        public virtual Location LocationA { get; set; }
        public virtual Location LocationB { get; set; }

    }
}
