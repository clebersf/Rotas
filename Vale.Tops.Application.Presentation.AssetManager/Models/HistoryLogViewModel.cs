using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class HistoryLogViewModel
    {

        public IEnumerable<vw_Agent_History_Performance> agenthistory { get; set; }

        public IEnumerable<Log> lgs { get; set; }

        [Display(Name = "Rota")]
        public DateTime DateTime { get; set; }
        [Display(Name = "Job")]
        public string Job { get; set; }
        [Display(Name = "Schedule")]
        public string Schedule { get; set; }
        [Display(Name = "Duração Conf.")]
        public int ScheduleDuration { get; set; }
        [Display(Name = "Duração Real")]
        public int CurrentDuration { get; set; }
        [Display(Name = "Performance")]
        public string Performance { get; set; }
        [Display(Name = "Mensagem")]
        public string Msg { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Inicio")]
        public DateTime? dhpi { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Fim")]
        public DateTime? dhpf { get; set; }

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

        [Display(Name = "Filtro Negado")]
        public string NegFilter { get; set; }
        [Display(Name = "Filtro ")]
        public string Filter { get; set; }

        [Display(Name = "Valor atual")]
        public string Current { get; set; }
        [Display(Name = "Tag")]
        public string TagName { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Horário")]
        public DateTime dh { get; set; }


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