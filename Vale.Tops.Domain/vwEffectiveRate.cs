using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class vwEffectiveRate : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime dh { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public string Destino { get; set; }
        public double txEfetivaDestino { get; set; }
        public string Origem { get; set; }
        public double txEfetivaOrigem { get; set; }
        public double txEfetivaOrigem10Min { get; set; }
        public double txEfetivaDestino10Min { get; set; }
        public double TotalOrigem { get; set; }
        public double TotalDestino { get; set; }
        public double txPeak { get; set; }
        public int QtdPeak { get; set; }
        public double TimePeak { get; set; }
        public double txL1 { get; set; }
        public double txL2 { get; set; }
        public double txL3 { get; set; }
        public double txL4 { get; set; }
        public double txL5 { get; set; }
        public double txL6 { get; set; }
        public double txL7 { get; set; }
        public string Berco { get; set; }
        public bool Final { get; set; }
        public string Operator { get; set; }
        public int Porao { get; set; }
    }
}
