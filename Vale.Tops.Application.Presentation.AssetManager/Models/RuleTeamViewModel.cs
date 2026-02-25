using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class RuleTeamViewModel
    {
        public IEnumerable<Rule> Rules { get; set; }
        public IEnumerable<Vale.Tops.Domain.Application> Applications { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public List<string> NameApplication { get; set; }
        public List<string> NameRule { get; set; }
        public List<string> NameGroupDns { get; set; }
        [Display(Name = "Aplicação")]
        public string Application { get; set; }
        [Display(Name = "Perfíl")]
        public string Rule { get; set; }
        [Display(Name = "Grupo DNS")]
        public string GroupDns { get; set; }
        public Tops.Domain.Type Type { get; set; }
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        public long RuleId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        public long ApplicationId { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}