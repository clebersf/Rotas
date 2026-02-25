using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Rotas
{
    /// <summary>
    /// Objeto Consistencia de rotas contemplando os atributos de um teste de consistencia de uma rota
    /// </summary>
    class Consist
    {
        //String completa da rota
        public virtual string Completa { get; set; }
        //Tipo de consistencia
        public virtual string Tipo { get; set; }
        //Equipamento alvo da consistencia
        public virtual string Eqp { get; set; }
        // Proximo equipamento da rota
        public virtual string Proximo { get; set; }
        // Status da consistencia
        public virtual string Status { get; set; }
    }
}
