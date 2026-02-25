namespace Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlarmFtv")]
    public partial class AlarmFtv : Entity
    {
        [Key]
        public virtual Guid EventID { get; set; }
        public int? EventType { get; set; }
        public string SourceName { get; set; }
        public string SourcePath { get; set; }
        public Guid? SourceID { get; set; }
        public string ServerName { get; set; }
        public Int64? TicksTimeStamp { get; set; }
        public DateTime? EventTimeStamp { get; set; }
        public string EventCategory { get; set; }
        public int? Severity { get; set; }
        public int? Priority { get; set; }
        public string Message { get; set; }
        public string ConditionName { get; set; }
        public string SubConditionName { get; set; }
        public string AlarmClass { get; set; }
        public bool? Active { get; set; }
        public bool? Acked { get; set; }
        public bool? EffDisabled { get; set; }
        public bool? Disabled { get; set; }
        public bool? EffSuppressed { get; set; }
        public bool? Suppressed { get; set; }
        public string PersonID { get; set; }
        public int? ChangeMask { get; set; }
        public double? InputValue { get; set; }
        public double? LimitValue { get; set; }
        public int? Quality { get; set; }
        public Guid? EventAssociationID { get; set; }
        public string UserComment { get; set; }
        public string ComputerID { get; set; }
        public string Tag1Value { get; set; }
        public string Tag2Value { get; set; }
        public string Tag3Value { get; set; }
        public string Tag4Value { get; set; }
        public bool? Shelved { get; set; }
        public DateTime? AutoUnshelveTime { get; set; }
        public string GroupPath { get; set; }

    }
}
