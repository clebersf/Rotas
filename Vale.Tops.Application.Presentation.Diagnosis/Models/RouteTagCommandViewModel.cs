using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vale.Tops.Application.Presentation.Diagnosis.ViewModels
{
    public class RouteTagCommandViewModel
    {
        public long Id { get; set; } // Id da Rota
        public string Route { get; set; }
        public string EquipmentType { get; set; } // Tipo do Equipamento (DAMPER, FEEDER, etc.)
        public string Asset { get; set; } // Nome do Equipamento
        public string TagName { get; set; }
        public string CommandStatus { get; set; } // Descrição do Veredito
        public bool IsPermitted { get; set; } // Status Booleano para visualização
    }
}