using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class RuleViewModel
    {
        public IEnumerable<Vale.Tops.Domain.Application> Applications { get; set; }
        public IEnumerable<Rule> Rules { get; set; }
        public Rule Rule { get; set; }
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Grupo DNS")]
        public string GroupDns { get; set; }
        public Vale.Tops.Domain.Application Application { get; set; }
        public IEnumerable<Tops.Domain.Type> Types { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public Tops.Domain.Type Type { get; set; }
        public string Name { get; set; }        
        public string Message { get; set; }
        public int lines { get; set; }
    }
}