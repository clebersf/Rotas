namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rTagWrite")]
    public partial class rTagWrite : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rTagWrite()
        {

        }
        [Key]
        public virtual long Id { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual bool Write { get; set; }

        public virtual string Value { get; set; }
    }
}
