using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class TagViewModel
    {
        public IEnumerable<Plc> Plcs { get; set; }
        public IEnumerable<Domain.Tag> Tags { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public Domain.Tag Tag { get; set; }
        public long PlcId { get; set; }
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Endereço")]
        public string Name { get; set; }

        [Display(Name = "Plc")]
        public string Plc { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}