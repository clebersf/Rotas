using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class TypeViewModel
    {
        public IEnumerable<Domain.Type> Types { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        [Display(Name = "Categoria")]
        public Domain.Type Type { get; set; }
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Categoria")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}