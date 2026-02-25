namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Log")]
    public partial class Log : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Log()
        {
            
        }
        [Key]
        public virtual Guid Id { get; set; }
        public long ApplicationId { get; set; }
        
        public string User { get; set; }

        public DateTime dh { get; set; }

        public string Message { get; set; }

        public virtual Application Application { get; set; }

        public virtual Location Location { get; set; }
        public long LocationId { get; set; }
    }
}
