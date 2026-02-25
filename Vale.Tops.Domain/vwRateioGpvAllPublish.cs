using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class vwRateioGpvAllPublish : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }

        [System.ComponentModel.DisplayName("Artist")]
        public Int64? CodRotaGpv { get; set; }
        /// <summary>
        ///  TCódigo da rota GPV
        /// </summary>
        public Guid? PassoId { get; set; }
        /// <summary>
        ///  Data inicial do passo do porão
        /// </summary>
        public DateTime? PassoDhInicio { get; set; }
        /// <summary>
        ///  Data final do passo do porão
        /// </summary>
        public DateTime? PassoDhFim { get; set; }
        /// <summary>
        ///  Passo finalizado
        /// </summary>
        public bool PassoFinalizado { get; set; }
        /// <summary>
        ///  Nome do berço do navio
        /// </summary>
        public string BercoNome { get; set; }
        /// <summary>
        ///  Nome do destino
        /// </summary>
        public string DestinoNome { get; set; }
        /// <summary>
        ///  Carga total do destino do passo
        /// </summary>
        public double? DestinoTotal { get; set; }
        /// <summary>
        ///  Nome da origem
        /// </summary>
        public string OrigemNome { get; set; }
        /// <summary>
        ///  Carga total da origem do passo
        /// </summary>
        public double? OrigemTotal { get; set; }
        /// <summary>
        ///  Data inicial da origem no passo
        ///  </summary>
        public DateTime? OrigemDhInicio { get; set; }
        /// <summary>
        ///  Data final da origem no passo
        ///  </summary>
        public DateTime? OrigemDhFim { get; set; }
        /// <summary>
        ///  Numero de vagões
        /// </summary>
        public int NVagao { get; set; }
        /// <summary>
        /// Balança utilizada
        /// </summary>
        public string Balanca { get; set; }
    }
}
