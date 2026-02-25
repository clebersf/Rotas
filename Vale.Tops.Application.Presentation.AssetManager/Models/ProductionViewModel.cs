using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class ProductionViewModel
    {
        public IEnumerable<vw_Presentation_Production_Boarding> vwProductionBoardingPresentations { get; set; }
        public IEnumerable<PresentationProductionBoarding> ProductionBoardingPresentations { get; set; }
        public IEnumerable<Domain.Type> Routes { get; set; }
        public IEnumerable<Domain.Production> Productions { get; set; }
        public IEnumerable<Domain.rProductionStock> ProductionStocks { get; set; }
        public IEnumerable<Domain.rProductionBoarding> ProductionBoardings { get; set; }
        public List<Domain.rProductionOrigin> ProductionOrigins { get; set; }

        public List<Domain.rProductionRoute> ProductionRoutes { get; set; }
        public List<Domain.Type> Berths { get; set; }
        public List<Domain.Type> Origins { get; set; }

        public List<Domain.Type> Destinations { get; set; }
        [Display(Name = "Origem")]
        public Origin Origin { get; set; }

        public Route Route { get; set; }

        [Display(Name = "CN")]
        public Destination Destination { get; set; }

        [Display(Name = "Berço")]
        public Domain.Type Berth { get; set; }
        public List<SelectedGuid> Selecteds { get; set; }
        [Display(Name = "Produção")]
        public Domain.Production Production { get; set; }
        public Domain.rProductionStock rProductionStock { get; set; }
        public Domain.rProductionBoarding rProductionBoarding { get; set; }
        public Domain.rProductionOrigin rProductionOrigin { get; set; }
        public Guid Id { get; set; }

        [Display(Name = "Data hora Inicial")]
        public DateTime? dhpi { get; set; }

        [Display(Name = "Data hora Final")]
        public DateTime? dhpf { get; set; }

        [Display(Name = "Data hora Inicial")]
        public DateTime? dhi { get; set; }

        [Display(Name = "Data hora Final")]
        public DateTime? dhf { get; set; }
        [Display(Name = "Baliza inicial")]
        public int GoalI { get; set; }
        [Display(Name = "Baliza Final")]
        public int GoalF { get; set; }
        [Display(Name = "Carga")]
        public double Load { get; set; }

        [Display(Name = "VVG1")]
        public string VVG1 { get; set; }

        [Display(Name = "VVG2")]
        public string VVG2 { get; set; }

        [Display(Name = "VVG3")]
        public string VVG3 { get; set; }

        [Display(Name = "ERG1")]
        public string ERG1 { get; set; }

        [Display(Name = "ERG2")]
        public string ERG2 { get; set; }

        [Display(Name = "ERG3")]
        public string ERG3 { get; set; }

        public long BerthId { get; set; }
        public bool Active{ get; set; }
        public bool Final { get; set; }
        public long RouteId { get; set; }

        public long OriginId { get; set; }

        public long DestinationId { get; set; }

        public Guid ProductionId { get; set; }
        [Display(Name = "Porão")]
        public int Compartment { get; set; }

        public string Message { get; set; }
        public int lines { get; set; }
    }

}