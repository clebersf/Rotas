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
    public class vwFlowPeakReclaimer : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }

        public string TypeResult { get; set; }
        public string Asset { get; set; }
        /// <summary>
        ///  TCódigo da rota GPV
        /// </summary>
        public double TimePeak { get; set; }
        /// <summary>
        ///  Data inicial do passo do porão
        /// </summary>
        public double MaxPeak { get; set; }
        /// <summary>
        ///  Data inicial do passo do porão
        /// </summary>
        public double EffectiveRate { get; set; }
        /// <summary>
        ///  Data final do passo do porão
        /// </summary>
        public string User { get; set; }

    }
}