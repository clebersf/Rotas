using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{

    public class RailRoadReportModel
    {

        public List<vwRailRoadReport> ReportLines { get; set; }

        public string Nome { get; set; }
        public int CorDuracaoRetirar { get; set; }
        public int CorDuracaoRolar { get; set; }
        public int CorEntreLotes { get; set; }
        public int CorPreparar { get; set; }
        [Display(Name = "Tempo retirar")]
        public TimeSpan hDuracaoRetirar { get; set; }
        [Display(Name = "Tempo rolar cheias")]
        public TimeSpan hDuracaoRolar { get; set; }
        [Display(Name = "Tempo entre lotes")]
        public TimeSpan hEntreLotes { get; set; }
        [Display(Name = "Término Desc.")]
        public TimeSpan hf { get; set; }
        [Display(Name = "Início Desc.")]
        public TimeSpan hi { get; set; }
        [Display(Name = "Prev. Bota-fora")]
        public TimeSpan hLivrar { get; set; }
        [Display(Name = "Tempo para drenagem")]
        public TimeSpan hPreparar { get; set; }
        [Display(Name = "Prev. término")]
        public TimeSpan hPrevTermino { get; set; }
        [Display(Name = "Horário vazias")]
        public TimeSpan hRetira { get; set; }
        [Display(Name = "Horário cheias")]
        public TimeSpan hRolar { get; set; }
        public bool LatchRetira { get; set; }
        public bool LatchRolar { get; set; }
        [Display(Name = "Qt. Rest.")]
        public int NCheio { get; set; }
        [Display(Name = "Qt. Desc.")]
        public int NDescarregado { get; set; }
        [Display(Name = "Qt. Vagões")]
        public int NVagao { get; set; }
        public string Status { get; set; }

        [Display(Name = "Tempo entre lotes")]
        public TimeSpan hEntreLotesAnt { get; set; }

        [Display(Name = "Tempo Prep. Cheias")]
        public TimeSpan hPrepararAnt { get; set; }

        [Display(Name = "Horário cheias")]
        public TimeSpan hRolarAnt { get; set; }

        [Display(Name = "Tempo rolar cheias")]
        public TimeSpan hDuracaoRolarAnt { get; set; }

        [Display(Name = "Início Desc.")]
        public TimeSpan hiAnt { get; set; }

        [Display(Name = "Qt. Vagões")]
        public int NVagaoAnt { get; set; }

        [Display(Name = "Término Desc.")]
        public TimeSpan hfAnt { get; set; }

        [Display(Name = "Tempo retirar")]
        public TimeSpan hDuracaoRetirarAnt { get; set; }

        [Display(Name = "Horário vazias")]
        public TimeSpan hRetiraAnt { get; set; }
    }

}