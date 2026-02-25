namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EventFtv")]
    public partial class EventFtv : Entity
    {
        [Key]
        public virtual string RecordID { get; set; }
        public string ParentID { get; set; }
        public Int16 SequenceNumber { get; set; }
        public DateTime? ChainTime { get; set; }
        public DateTime? TimeStmp { get; set; }
        public string MessageText { get; set; }
        public Int16 Audience { get; set; }
        public Int16 Severity { get; set; }
        public Int16 Verbosity { get; set; }
        public string Location { get; set; }
        public string Provider { get; set; }
        public string UserID { get; set; }
        public string UserFullName { get; set; }


    }
}
