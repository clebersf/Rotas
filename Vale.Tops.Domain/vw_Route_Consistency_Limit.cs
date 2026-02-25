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
    public class vw_Route_Consistency_Limit : Entity
    {
        [Key]
        public long VwId { get; set; }
        public long Id { get; set; }
        public long LocationId { get; set; }
        public string Route { get; set; }
        public string Conveyor { get; set; }
        public Int32 Count { get; set; }
        public double Limit { get; set; }
        public string Veredict { get; set; }
        public Int32 BoolVeredict { get; set; }     
        public string Type { get; set; }

    }
}
