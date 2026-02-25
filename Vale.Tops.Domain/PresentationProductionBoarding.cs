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
    /// Integração do sistema de rateio do porto tu com o sistema GPV portos
    /// Parâmetro de entrada: Data inicial e Data Final
    /// Retorno: Lista de informações do Rateio dos passos do navio
    /// do ponto de vista da origem.
    /// </summary>
    /// <param name="CodRotaGpv"> Data inicial da consulta</param>
    /// <param name="PassoId"> Data final da consulta</param>
    /// <returns>Lista de informações do Rateio dos passos do navio
    /// do ponto de vista da origem.
    /// </returns>
    /// 
    
    public class PresentationProductionBoarding : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        [Key]
        public virtual Guid Id { get; set; }

        public virtual long Cod { get; set; }

        /// <summary>
        ///  Data inicial do passo do porão
        /// </summary>
        public DateTime? dhi { get; set; }
        /// <summary>
        ///  Data final do passo do porão
        /// </summary>
        public DateTime? dhf     { get; set; }

        /// <summary>
        ///  Nome do berço do navio
        /// </summary>
        public string Berth { get; set; }

        /// <summary>
        ///  Destino
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Carga do porão
        /// </summary>
        public double Load { get; set; }

        /// <summary>
        ///  Nome do berço do navio
        /// </summary>
        public int Compartment { get; set; }

        /// <summary>
        ///  Virador de vagões 1
        /// </summary>
        public double VVG1 { get; set; }

        /// <summary>
        ///  Virador de vagões 2
        /// </summary>
        public double VVG2 { get; set; }

        /// <summary>
        ///  Virador de vagões 3
        /// </summary>
        public double VVG3 { get; set; }

        /// <summary>
        ///  Empilhadeira e recuperadora 1
        /// </summary>
        public double ERG1 { get; set; }

        /// <summary>
        ///  Empilhadeira e recuperadora 2
        /// </summary>
        public double ERG2 { get; set; }

        /// <summary>
        ///  Empilhadeira e recuperadora 3
        /// </summary>
        public double ERG3 { get; set; }

        /// <summary>
        ///  Passo finalizado
        /// </summary>
        public bool Final { get; set; }
        
    }
}
