using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class InstrumentMeasureViewModel
    {
        public IEnumerable<rInstrumentMeasure> rInstrumentMeasures { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
        public rInstrumentMeasure rInstrumentMeasure { get; set; }
        public List<string> Resources { get; set; }
        public long Id { get; set; }
        public long TagId { get; set; }
        public long LocationId { get; set; }
        [Display(Name = "Tag")]
        public string Tag { get; set; }
        [Display(Name = "Instrumento")]
        public string Location { get; set; }
        [Display(Name = "Value")]
        public string Value { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
    }

}