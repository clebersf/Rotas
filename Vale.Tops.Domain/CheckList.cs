namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckList")]
    public partial class CheckList : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CheckList()
        {
            
        }

        [Key]
        public virtual Guid Id { get; set; }

        public virtual DateTime dh { get; set; }

        public long LocationId { get; set; }

        public virtual Location Location { get; set; }

        public int Yes { get; set; }

        public int No { get; set; }

        public int NA { get; set; }

        public int Max { get; set; }

        public string Note { get; set; }

        public string User { get; set; }
    }
}
