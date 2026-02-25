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
    [Table("vw_RouteLog")]
    public class vwRouteLog : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }
        /// <summary>
        ///  Matricula
        /// </summary>
        public DateTime Horário { get; set; }

        /// <summary>
        ///  Nome do Uauário
        /// </summary>
        public string Evento { get; set; }

        /// <summary>
        ///  Conta do domínio
        /// </summary>
        public string Computador { get; set; }

        /// <summary>
        ///  Descrição do localInstalacao
        /// </summary>
        public string Rota { get; set; }  

    }
}
