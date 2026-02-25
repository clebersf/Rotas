namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rTagInformation")]
    public partial class rTagInformation : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rTagInformation()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
