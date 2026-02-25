namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vwGraphJson")]
    public partial class vwGraphJson : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vwGraphJson()
        {            
        }
        [Key]
        public virtual long Id { get; set; }
        public string Json { get; set; }

        
    }
}
