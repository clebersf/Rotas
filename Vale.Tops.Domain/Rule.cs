namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rule")]
    public partial class Rule : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rule()
        {
            
        }
        [Key]
        public long Id { get; set; }
        public string Description { get; set; }

        public string GroupDns { get; set; }

        public virtual rRuleNavbar rRuleNavbar { get; set; }

        public virtual Application Application { get; set; }
        public long ApplicationId { get; set; }



    }
}
