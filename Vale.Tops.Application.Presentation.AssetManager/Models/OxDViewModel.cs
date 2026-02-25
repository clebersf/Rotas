using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class OxDViewModel
    {
        public IEnumerable<OxD> OxDs { get; set; }
        public IEnumerable<Tops.Domain.Location> Locations_1 { get; set; }
        public IEnumerable<Tops.Domain.Location> Locations_2 { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public List<string> Resources { get; set; }
        public Tops.Domain.Location Location { get; set; }
        public long Id { get; set; }
        public long OriginId { get; set; }
        public long DestinationId { get; set; }
        [Display(Name = "Origem")]
        public string Origin { get; set; }
        [Display(Name = "Destino")]
        public string Destination { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}