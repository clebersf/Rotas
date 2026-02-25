using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class OperationPerformanceViewModel
    {
        public IEnumerable<vwEffectiveRate>  EffectiveRates { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        [Display(Name = "Sim")]
        public bool Yes { get; set; }
        public long Id { get; set; }
        public DateTime dh { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public string Destino { get; set; }
        [Display(Name = "tx Ef. CN (ton/h)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true)]
        public double txEfetivaDestino { get; set; }
        public string Origem { get; set; }
        [Display(Name = "Porão")]
        public int Porao { get; set; }

        [Display(Name = "Tot CN")]
        public double TotalDestino { get; set; }
        [Display(Name = "Tot RC")]
        public double TotalOrigem { get; set; }

        [Display(Name = "tx Ef. RC (ton/h)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true)]
        public double txEfetivaOrigem { get; set; }
        [Display(Name = "tx Ef. RC 10 min (ton/h)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true)]
        public double txEfetivaOrigem10Min { get; set; }
        [Display(Name = "tx Ef. CN 10 min (ton/h)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true)]
        public double txEfetivaDestino10Min { get; set; }
        [Display(Name = "tx Ef. RC Projetada")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public double txEfetivaOrigemProjetada { get; set; }
        [Display(Name = "Tx Pico (pico/h)")]
        public double txPeak { get; set; }
        [Display(Name = "Qtd Pico")]
        public double QtdPeak { get; set; }
        [Display(Name = "Tempo Pico (h)")]
        public double TimePeak { get; set; }
        [Display(Name = " tx Banc. x (pico/h)")]
        public double txL1 { get; set; }
        [Display(Name = " tx Banc. 1 (pico/h)")]
        public double txL2 { get; set; }
        [Display(Name = " tx Banc. 2 (pico/h)")]
        public double txL3 { get; set; }
        [Display(Name = " tx Banc. 3 (pico/h)")]
        public double txL4 { get; set; }
        [Display(Name = " tx Banc. 4 (pico/h)")]
        public double txL5 { get; set; }
        [Display(Name = " tx Banc. 5 (pico/h)")]
        public double txL6 { get; set; }
        [Display(Name = " tx Banc. 7 (pico/h)")]
        public double txL7 { get; set; }
        public string Berco { get; set; }
        [Display(Name = "Mat. Operador")]
        public string Operator { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Inicio")]
        public DateTime? dhpi { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Fim")]
        public DateTime? dhpf { get; set; }

        public long LocationId { get; set; }

    }

}