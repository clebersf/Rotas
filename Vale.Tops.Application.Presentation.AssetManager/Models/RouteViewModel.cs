using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Application.Presentation.AssetManager.Controllers;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class RouteViewModel
    {
        public IEnumerable<Domain.rRouteOxD> Routes { get; set; }
        public IEnumerable<Domain.rRouteActive> Actives { get; set; }
        public IEnumerable<Log> logs { get; set; }
        public IEnumerable<RouteSequence> Sequences { get; set; }
        public IEnumerable<Domain.Type> OxDs { get; set; }
        public IEnumerable<Location> eqps { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public Domain.Route Route { get; set; }
        public bool Active { get; set; }
        public long OxDId { get; set; }
        public long LocationId { get; set; }
        public long Id { get; set; }

        [Display(Name = "Data")]
        public DateTime dh { get; set; }

        [Display(Name = "Equipemanto")]
        public string Name { get; set; }

        [Display(Name = "Ordem")]
        public int Order { get; set; }

        [Display(Name = "Evento")]
        public string Event { get; set; }

        [Display(Name = "Computador")]
        public string User { get; set; }

        [Display(Name = "OXD")]
        public string OxD { get; set; }
        [Display(Name = "Resumida")]
        public string Reduced { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}