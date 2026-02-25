namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Logbook")]
    public partial class Logbook : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Logbook()
        {
            rLogbookHistory = new HashSet<rLogbookHistory>();
        }

        

        [Key]
        public virtual Guid Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rLogbookHistory> rLogbookHistory { get; set; }
        public virtual rLogbookEPEE rLogbookEPEE { get; set; }

        public virtual rLogbookVV rLogbookVV { get; set; }
        public long? LocationId { get; set; }

        public DateTime? dh { get; set; }

        public string User { get; set; }

        public string AreaSelected { get; set; }

        public string Observation { get; set; }

        public int? LandmarkInitial { get; set; }

        public int? LandmarkFinal { get; set; }

        public int? WeightAsset { get; set; }

        public int? WeightConjunct { get; set; }

        public int? ShipCradle { get; set; }

        public int? Hold { get; set; }

        public string Cod { get; set; }

        public string Material { get; set; }

        public int? Quantity { get; set; }

        public int? FlowMeasured { get; set; }

        public int? FlowReal
        {
            get
            {
                if (hf == hi && mf == mi)
                {
                    return 0;
                }
                else if (hf > hi || mf > mi)
                {
                    int? _weightAsset = WeightAsset * 60;
                    int? _hmf = (hf * 60 + mf);
                    int? _hmi = (hi * 60 + mi);
                    int? _total = _weightAsset / (_hmf - _hmi);
                    return _total;
                }
                else
                {
                    return -1;
                }
            }
            set { }
        }

        public bool? isFail { get; set; }

        public int? hi { get; set; }

        public int? mi { get; set; }

        public int? hf { get; set; }

        public int? mf { get; set; }

        public string Asset { get; set; }

        public string Conjunct { get; set; }

        public virtual Location Location { get; set; }

        //Empilhadeiras

        public bool? MaterialQuality { get; set; }

        public bool? RestrictRoute { get; set; }

        public bool? StackEnd { get; set; }

        public bool? LmInitialNorthDirection { get; set; }

        public bool? LmFinalNorthDirection { get; set; }

        public bool? IsHopper { get; set; }

        public bool? LowFlowOrParalization { get; set; }

        public int? LotNumber { get; set; }

        public int? NumberOfWagons { get; set; }

        public int? LotPrefix { get; set; }

        public int? Steps { get; set; }

        public int? LotSLandmarkInitial { get; set; }

        public int? LotSLandmarkFinal { get; set; }

        public int? hP0 { get; set; }

        public int? mP0 { get; set; }

        public int? hUsina { get; set; }

        public int? mUsina { get; set; }

        public int? hAcoplado { get; set; }

        public int? mAcoplado { get; set; }

        public string FlowObservation { get; set; }

        public string StackingWay { get; set; }

        public string OutOfStack { get; set; }

        //Viradores
        public string User2 { get; set; }
        public bool? RestrictVV { get; set; }
    }
}
