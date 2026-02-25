// Local: Vale.Tops.Application.Presentation.Diagnosis/ViewModels/RouteConsistencyViewModel.cs

using System;
using System.Collections.Generic;

namespace Vale.Tops.Application.Presentation.Diagnosis.ViewModels
{
    // Model para o Detalhe da Falha (O PORQUÊ)
    public class RouteConsistencyDetailViewModel
    {
        public string Equipamento { get; set; } // Ex: C_101, DAMPER_3
        public string MensagemDeErro { get; set; } // O retorno da função fn_Consistency_* SQL
    }

    // Model Principal para a Linha da Tabela (O QUÊ e ONDE)
    public class RouteConsistencyViewModel
    {
        public long RouteId { get; set; }
        public string NomeCompleto { get; set; }
        public string NomeResumido { get; set; }
        public string TipoRota { get; set; } // "Ativa" ou "Fila"
        public bool Consistente { get; set; }
        public string StatusConsistencia => Consistente ? "SIM" : "NÃO"; // Propriedade formatada
        public DateTime UltimaAtualizacao { get; set; }

        // Lista de detalhes da falha (mostra apenas se Consistente for false)
        public List<RouteConsistencyDetailViewModel> DetalhesInconsistencia { get; set; }
    }
}