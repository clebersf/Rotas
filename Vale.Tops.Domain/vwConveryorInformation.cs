using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    /// <summary>
    /// Integração para sistema fully. Dados das Correias tranportadoras
    /// </summary>
    /// <returns>Integração para sistema fully. Dados das Correias tranportadoras
    /// </returns>
    [Table("vwConveyorInformation ")]
    public class vwConveyorInformation : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>
        ///  Conveyor name
        /// </summary>
        public string Equipment { get; set; }
        /// <summary>
        ///  Conveyor capacity
        /// </summary>
        public double? Capacity { get; set; }
        /// <summary>
        ///  Conveyor lenght
        /// </summary>
        public double? Lenght { get; set; }
        /// <summary>
        ///  Conveyor Speed
        /// </summary>
        public double? ConveyorSpeed { get; set; }
        /// <summary>
        ///  Conveyor Scale
        /// </summary>
        public string BeltScale { get; set; }
        /// <summary>
        ///  Conveyor Scale Position
        /// </summary>
        public double? BeltScalePosition { get; set; }
       

    }
}
