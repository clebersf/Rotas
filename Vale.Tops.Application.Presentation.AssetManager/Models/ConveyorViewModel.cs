using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Application.Presentation.AssetManager.Controllers;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class ConveyorViewModel
    {
        public IEnumerable<Tops.Domain.Location> Locations { get; set; }
        public IEnumerable<Conveyor> Conveyors { get; set; }
        public IEnumerable<ConveyorOutputPosition> OutputPositions { get; set; }
        public IEnumerable<ConveyorInputPosition> InputPositions { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public IEnumerable<Location> Outputs { get; set; }
        public IEnumerable<Location> Inputs { get; set; }
        public List<string> Resources { get; set; }
        public Tops.Domain.Location Location { get; set; }

        [Display(Name = "Saidas")]
        public bool IsOutputPosition { get; set; }
        [Display(Name = "Entradas")]
        public bool IsInputPosition { get; set; }
        public long LocationId { get; set; }
        public long Id { get; set; }
        [Display(Name = "Ativo")]
        public string Name { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Balança")]
        public string Scale { get; set; }


        [Display(Name = "Comprimento")]
        public double Lenght { get; set; }

        [Display(Name = "Capacidade")]
        public double Capacity { get; set; }

        [Display(Name = "Consumo")]
        public double Consumption { get; set; }

        [Display(Name = "Capacidade")]
        public double Limit { get; set; }

        [Display(Name = "Velocidade")]
        public double Speed { get; set; }

        [Display(Name = "Posição")]
        public double Position { get; set; }

        public string Message { get; set; }
        public int lines { get; set; }
    }

}