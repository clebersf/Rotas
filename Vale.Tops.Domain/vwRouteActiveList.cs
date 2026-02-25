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
    /// Integração para sistema fully. Dados das Rotas de Correias tranportadoras
    /// </summary>
    /// <returns>Integração para sistema fully. Dados das Rotas de Correias tranportadoras
    /// </returns>
    [Table("vwRouteActiveList")]
    public class vwRouteActiveList : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }
        /// <summary>
        ///  Código da rota GPV
        /// </summary>
        public Int64? GpvCode { get; set; }
        /// <summary>
        ///  Route´s string
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        ///  Conveyor name
        /// </summary>
        public string Equipment { get; set; }
        /// <summary>
        ///  Sequence in route
        /// </summary>
        public int Sequence { get; set; }
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
        /// <summary>
        ///  Input Position
        /// </summary>
        public double? InputPosition { get; set; }
        /// <summary>
        ///  Nome do destino
        /// </summary>
        public double? OutputPosition { get; set; }
        

    }
}
