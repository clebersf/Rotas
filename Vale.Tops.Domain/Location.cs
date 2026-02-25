namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            // Atribuições de relacionamentos 1:N de Location
            Parent = new HashSet<Location>();
            Logbook = new HashSet<Logbook>();
            CheckList = new HashSet<CheckList>();

            LocationA = new HashSet<Graph>();
            LocationB = new HashSet<Graph>();

            rRouteSequence = new HashSet<rRouteSequence>();

            rRouteOxD = new HashSet<rRouteOxD>();

            rProductionBoarding = new HashSet<rProductionBoarding>();
            rProductionRoute = new HashSet<rProductionRoute>();

            rLocationHistory = new HashSet<rLocationHistory>();
            Logs = new HashSet<Log>();
            rLocationDataHistory = new HashSet<rLocationDataHistory>();

            rConveyorOutputPosition = new HashSet<rConveyorOutputPosition>();
            rConveyorInputPosition = new HashSet<rConveyorInputPosition>();
            rConveyorScale = new HashSet<rConveyorScale>();
        }
        [Key]
        public virtual long Id { get; set; }
        public long? ParentId { get; set; }

        public long? TypeId { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }


        // Chaves estrangeiras de Location 1:1
        public virtual Location _Location { get; set; }

        public virtual Type Type { get; set; }

        // Recursividade Location 1:N

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Parent { get; set; }


        // Objetos do tipo Location - Relacionamentos 1:N

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Logbook> Logbook { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckList> CheckList { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rLocationDataHistory> rLocationDataHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rProductionBoarding> rProductionBoarding { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rRouteSequence> rRouteSequence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rRouteOxD> rRouteOxD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rProductionRoute> rProductionRoute { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rLocationHistory> rLocationHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log> Logs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Graph> LocationA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Graph> LocationB { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rConveyorScale> rConveyorScale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rConveyorOutputPosition> rConveyorOutputPosition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rConveyorInputPosition> rConveyorInputPosition { get; set; }

        public virtual rRouteReplace rRouteReplace { get; set; }
        public virtual rRouteActive rRouteActive { get; set; }
        public virtual rRouteQueue rRouteQueue { get; set; }
        public virtual rRouteTravel rRouteTravel { get; set; }

    }
}
