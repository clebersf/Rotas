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
    public class vw_Plc_Read_Time : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }

        [System.ComponentModel.DisplayName("Artist")]
        public String Name { get; set; }
        /// <summary>
        ///  Nome do ativo
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        ///  Tempo mínimo que a leitura atingiu (Dado em milissegundos)
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        ///  Tempo máximo que a leitura atingiu (Dado em milissegundos)
        /// </summary>
        public float Avg { get; set; }
        /// <summary>
        ///  Média de tudo
        /// </summary>
        public int StdDev { get; set; }
        /// <summary>
        ///  Nome do berço do navio
        /// </summary>
        public int QtdTagsRead { get; set; }
        /// <summary>
        ///  Nome do destino
        /// </summary>
        public int TagGroupId { get; set; }
        /// <summary>
        ///  Id do Grupo de Tags
        /// </summary>
        public string TagGroup { get; set; }
        /// <summary>
        ///  Grupo da Tag
        /// </summary>
        public int TimePerRead { get; set; }
        /// <summary>
        ///  Tempo por leitura (Dado em milissegundos)
        /// </summary>
        public int MonitorTime { get; set; }
        /// <summary>
        ///  Data inicial da origem no passo
        ///  </summary>
        public float NominalQtdRead { get; set; }
        /// <summary>
        ///  Numero de vagões
        /// </summary>
        public double RealQtdRead { get; set; }
        /// <summary>
        /// Balança utilizada
        /// </summary>
    }
}
