using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class EventFtvViewModel
    {
        public IEnumerable<EventFtv> Events { get; set; }
        public int TypeId { get; set; }
        public string Filter { get; set; }
        public Guid RecordID { get; set; }
        public string ParentID { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime ChainTime { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime TimeStmp { get; set; }
        [Display(Name = "Descrição")]
        public string MessageText { get; set; }
        public int Audience { get; set; }
        public int Severity { get; set; }
        public int Verbosity { get; set; }
        [Display(Name = "Computador")]
        public string Location { get; set; }
        public string Provider { get; set; }
        [Display(Name = "Conta")]
        public string UserID { get; set; }
        [Display(Name = "Usuário")]
        public string UserFullName { get; set; }



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