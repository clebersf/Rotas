namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rRuleNavbar")]
    public partial class rRuleNavbar : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public virtual long Id { get; set; }
        public string NameOption { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ImageClass { get; set; }

        public bool Status { get; set; }

        public virtual Rule Rule { get; set; }
    }
}
