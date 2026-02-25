namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLocationIdxOxD")]
    public partial class rLocationIdxOxD : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rLocationIdxOxD()
        {
 
        }
        [Key]
        public virtual long Id { get; set; }
        public virtual int Idx { get; set; }
        public virtual Location Location { get; set; }


    }
}
