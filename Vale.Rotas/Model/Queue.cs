using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Rotas
{
    /// <summary>
    /// Objeto de fila de rota com os atributos das rotas que compõe a fila de rotas
    /// </summary>
    class Queue
    {
        // Id da rota
        public long Id { get; set; }
        // Codigo GPV da rota
        public long Cod { get; set; }
        //String completa da rota
        public string Completa { get; set; }
        // String rota resumida
        public string Resumida { get; set; }
        // Flag que sinaliza que a rota está consistente
        public string Consistente { get; set; }
        //Data da última leitura do banco de dados
        public DateTime Atualização { get; set; }
    }
}
