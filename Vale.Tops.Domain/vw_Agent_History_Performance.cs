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
    public class vw_Agent_History_Performance : Entity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Job { get; set; }
        public string Schedule { get; set; }
        public int ScheduleDuration { get; set; }
        public int CurrentDuration { get; set; }
        public string Performance { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }

    }
}
