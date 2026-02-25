using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class RouteActiveViewModel
    {
        public IEnumerable<rRouteActive> ActiveRoutes { get; set; }
        public IEnumerable<rRouteReplace> Replaces { get; set; }
        public IEnumerable<Route> Routes { get; set; }
        public IEnumerable<Domain.Type> OxDs { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public Domain.Route Route { get; set; }
        public DateTime dh { get; set; }
        public long OxDId { get; set; }
        public long Id { get; set; }
        [Display(Name = "Ativo")]
        public bool Active { get; set; }
        [Display(Name = "Rota Substituta")]
        public string Replace { get; set; }
        [Display(Name = "OXD")]
        public string OxD { get; set; }
        [Display(Name = "Descrição")]
        public string Name { get; set; }
        [Display(Name = "Completa")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}