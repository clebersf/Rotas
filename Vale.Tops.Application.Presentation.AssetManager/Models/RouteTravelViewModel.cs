using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class RouteTravelViewModel
    {
        public IEnumerable<rRouteTravel> RouteTravels { get; set; }
        public IEnumerable<Route> Routes { get; set; }
        public IEnumerable<Domain.Type> OxDs { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public Domain.Route Route { get; set; }
        public long OxDId { get; set; }
        public long Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Constante")]
        public double Constante { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Variável")]
        public double Variable { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Posição Inicial")]
        public double GoalI { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Posição Final")]
        public double GoalF { get; set; }

        public string OxD { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}