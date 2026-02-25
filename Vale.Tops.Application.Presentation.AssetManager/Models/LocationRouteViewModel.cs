using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class LocationRouteViewModel
    {
        //public IEnumerable<rLocationRoute> rLocationRoutes { get; set; }
        public IEnumerable<Route> Routes { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public List<string> Resources { get; set; }
        public long Id { get; set; }
        public long RouteId { get; set; }
        public long LocationId { get; set; }
        [Display(Name = "Rota")]
        public string Route { get; set; }
        [Display(Name = "Instrumento")]
        public string Location { get; set; }
        [Display(Name = "Descrição")]
        public string Value { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}