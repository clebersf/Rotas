using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class HomeViewModel
    {        
        public HomeViewModel ()
        {
            Criticality0 = 0;
            Criticality1 = 0;
            Criticality2 = 0;
            Criticality3 = 0;
        }
        public DateTime? dh { get; set; }
        public DateTime? dhf { get; set; }
        public string Doy { get; set; }
        public int Criticality0 { get; set; }
        public int Criticality1 { get; set; }
        public int Criticality2 { get; set; }
        public int Criticality3 { get; set; }
        public int NotAuthorized { get; set; }
        public int Total { get { return Criticality0 + Criticality1 + Criticality2 + Criticality3 + NotAuthorized; } }
        public string Name { get; set; }
        public int Change { get; set; }
        public int Delete { get; set; }
        public int NewTag { get; set; }
        public int Value { get; set; }
    }

    
}
