using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class ReversalViewModel
    {
        public IEnumerable<Tops.Domain.Location> Reversals { get; set; }
        public IEnumerable<Tops.Domain.Location> Eligible { get; set; }
        public IEnumerable<Tops.Domain.Location> References { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public List<string> Resources { get; set; }
        public Tops.Domain.Location Location { get; set; }
        public long Id { get; set; }
        public long LocRefId { get; set; }
        [Display(Name = "Alimentador")]
        public string Name { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public string TypeRef { get; set; }
        public int lines { get; set; }
    }

}