//namespace Vale.Tops.Domain
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    [Table("rRouteConveyor")]
//    public partial class rRouteConveyor : Entity
//    {
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
//        public rRouteConveyor()
//        {

//        }
//        [Key]
//        public virtual long Id { get; set; }

//        public virtual int order { get; set; }

//        public virtual long ConveyorId { get; set; }

//        public virtual Conveyor Conveyor { get; set; }

//        public virtual Route Route { get; set; }
//    }
//}
