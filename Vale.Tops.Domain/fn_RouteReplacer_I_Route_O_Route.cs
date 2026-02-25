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
    public class fn_RouteReplacer_I_Route_O_Route : Entity
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual int GpvCode { get; set; }
        public virtual string Route { get; set; }
        public virtual string Reduced { get; set; }
        public virtual string Rules { get; set; }
        public virtual int Consistency { get; set; }

    }
}
