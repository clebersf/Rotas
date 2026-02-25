using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Rotas
{
    /// <summary>
    /// Objeto de rota com seus atributos
    /// </summary>
    class Route
    {
        // Id da rota
        public long Id { get; set; }
        // Codigo GPV da rota
        //public long Cod { get; set; }
        //String completa da rota
        public string Completa { get; set; }
        // String rota resumida
        public string Resumida { get; set; }
        // Rota que irá substituir a rota atual (em caso de substituição de rotas)
        public virtual string Substituta { get; set; }

    }
}
