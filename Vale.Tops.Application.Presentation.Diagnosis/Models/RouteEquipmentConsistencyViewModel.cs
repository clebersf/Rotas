// Local sugerido: Vale.Tops.Application.Presentation.Diagnosis/ViewModels/RouteEquipmentConsistencyViewModel.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace Vale.Tops.Application.Presentation.Diagnosis.ViewModels
{
    /// <summary>
    /// Modelo para unificar as inconsistências por equipamento (Damper, Feeder, Reversal, Tripper).
    /// </summary>
    public class RouteEquipmentConsistencyViewModel
    {
        [Display(Name = "Tipo de Inconsistência")]
        public string InconsistencyType { get; set; }

        [Display(Name = "Rota ID")]
        public long RouteId { get; set; }

        [Display(Name = "Rota")]
        public string RouteName { get; set; }

        [Display(Name = "Equipamento Atual")]
        public string AssetCurrentName { get; set; }

        [Display(Name = "ID Equip. Atual")]
        public long AssetCurrentId { get; set; }

        [Display(Name = "Equipamento Seguinte")]
        public string AssetNextName { get; set; }

        [Display(Name = "ID Equip. Seguinte")]
        public long AssetNextId { get; set; }

        [Display(Name = "Veredito (Descrição)")]
        public string VeredictDescription { get; set; }

        // Atributos Específicos para Damper, Feeder, Reversal
        [Display(Name = "ID de Referência")]
        public long? ReferenceId { get; set; } // Nullable para views que não o possuem

        [Display(Name = "Nome da Tag")]
        public string TagName { get; set; }

        [Display(Name = "Valor da Tag (D/F/R)")]
        public string TagValue { get; set; } // Usado por Damper/Feeder/Reversal

        // Atributos Específicos para Tripper
        [Display(Name = "Modo (Código)")]
        public int? ModeCode { get; set; } // Nullable para views que não o possuem

        [Display(Name = "Modo (Descrição)")]
        public string DescriptionMode { get; set; }

        [Display(Name = "Valor Atual (Tripper)")]
        public string TripperCurrentValue { get; set; } // Usado por Tripper (campo 'Current')
    }
}