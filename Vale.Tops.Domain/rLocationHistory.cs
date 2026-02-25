namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLocationHistory")]
    public partial class rLocationHistory : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public virtual long Id { get; set; }
        public string Resource { get; set; }

        public virtual Location Location { get; set; }

        public Guid HistoryId { get; set; }

        public virtual History History { get; set; }
    }
}
