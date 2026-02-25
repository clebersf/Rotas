using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class AlarmFtvViewModel
    {
        public IEnumerable<AlarmFtv> Alarms { get; set; }

        public int TypeId { get; set; }
        [Display(Name = "Filtro")]
        public string Filter { get; set; }
        public Guid EventID { get; set; }
        public int EventType { get; set; }
        [Display(Name = "Tag")]
        public string SourceName { get; set; }
        [Display(Name = "Aplicação")]
        public string SourcePath { get; set; }
        public Guid SourceID { get; set; }
        [Display(Name = "Cat.")]
        public string ServerName { get; set; }
        public int TicksTimeStamp { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime EventTimeStamp { get; set; }
        public string EventCategory { get; set; }
        public int Severity { get; set; }
        public int Priority { get; set; }
        [Display(Name = "Alarme")]
        public string Message { get; set; }
        public string ConditionName { get; set; }
        public string SubConditionName { get; set; }
        public string AlarmClass { get; set; }
        [Display(Name = "Ativo")]
        public bool Active { get; set; }
        public bool Acked { get; set; }
        public bool EffDisabled { get; set; }
        public bool Disabled { get; set; }
        public bool EffSuppressed { get; set; }
        public bool Suppressed { get; set; }
        public string PersonID { get; set; }
        public int ChangeMask { get; set; }
        [Display(Name = "Valor da entrada")]
        public double InputValue { get; set; }
        public double LimitValue { get; set; }
        public int Quality { get; set; }
        public Guid EventAssociationID { get; set; }
        public string UserComment { get; set; }
        public string ComputerID { get; set; }
        public string Tag1Value { get; set; }
        public string Tag2Value { get; set; }
        public string Tag3Value { get; set; }
        public string Tag4Value { get; set; }
        public bool Shelved { get; set; }
        public DateTime AutoUnshelveTime { get; set; }
        public string GroupPath { get; set; }


        public Domain.Type Type { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Inicio")]
        public DateTime? dhpi { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Fim")]
        public DateTime? dhpf { get; set; }
        public string ViewMessage { get; set; }
        public int lines { get; set; }
    }

}