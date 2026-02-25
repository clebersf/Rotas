using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class RouteDiagnosticViewModel
    {
        public IEnumerable<vw_Route_Consistency_Feeder> cfeeders { get; set; }
        public IEnumerable<vw_Route_Consistency_Tripper> ctrippers{ get; set; }
        public IEnumerable<vw_Route_Consistency_Reversal> creversals{ get; set; }
        public IEnumerable<vw_Route_Tag_Feeder_Permission> permfeeder { get; set; }

        public IEnumerable<vw_Agent_History_Performance> agenthistory { get; set; }

        public IEnumerable<Log> lgs { get; set; }
        [Display(Name = "Id")]
        public long VwId { get; set; }

        [Display(Name = "TagId")]
        public long TagId { get; set; }

        [Display(Name = "Id Rota")]
        public long Id { get; set; }
        [Display(Name = "Rota")]
        public string Route { get; set; }
        [Display(Name = "Equipamento Atual")]
        public string AssetCurrent { get; set; }
        [Display(Name = "Proximo equipamento")]
        public string AssetNext { get; set; }
        [Display(Name = "Veredito")]
        public string Veredict { get; set; }
        [Display(Name = "Valor desejado")]
        public string Desired { get; set; }
        [Display(Name = "Modo do triper")]
        public string DescriptionMode { get; set; }

        [Display(Name = "Filtro evento Negado")]
        public string NegFilter { get; set; }
        [Display(Name = "Filtro evento")]
        public string Filter { get; set; }

        [Display(Name = "Valor atual")]
        public string Current { get; set; }
        [Display(Name = "Tag")]
        public string TagName { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Horário")]
        public DateTime dh { get; set; }
        [Display(Name = "Evento")]
        public string Msg { get; set; }

        [Display(Name = "Computador")]
        public string User { get; set; }


        [Display(Name = "Valor")]
        public string TagValue { get; set; }
        [Display(Name = "Tipo")]
        public string Type { get; set; }
        [Display(Name = "Valor desejado")]
        public string Reference { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}