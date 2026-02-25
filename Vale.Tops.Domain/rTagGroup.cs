namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rTagGroup")]
    public partial class rTagGroup : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rTagGroup()
        {
            Parent = new HashSet<rTagGroup>();
        }

        [Key]
        public virtual long Id { get; set; }
        public long? ParentId { get; set; }
        public long? TagId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rTagGroup> Parent { get; set; }
        public virtual rTagGroup _rTagGroup { get; set; }

        public virtual Tag Tag { get; set; }
        public string WindowsService { get; set; }

        public string SqlProcedure { get; set; }

        public string OpcServer { get; set; }

        public string AddrOpcServer { get; set; }

        public string Url_Read { get; set; }

        public string Url_Write { get; set; }

        public double Rate { get; set; }


    }
}
