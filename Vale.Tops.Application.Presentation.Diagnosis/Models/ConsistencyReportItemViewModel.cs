using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vale.Tops.Application.Presentation.Diagnosis.ViewModels
{
    // ConsistencyReportItem.cs (Localizado em ViewModels ou Domain)

    public class ConsistencyReportItemViewModel
    {
        public long Id { get; set; }
        public string EquipmentName { get; set; }
        public string InconsistencyType { get; set; }
        public int ConsistencyValue { get; set; } // Valor 0 ou 1
    }
}