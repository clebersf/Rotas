using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class vwRailRoadReport : Entity
    {
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; }
        public int CorDuracaoRetirar { get; set; }
        public int CorDuracaoRolar { get; set; }
        public int CorEntreLotes { get; set; }
        public int CorPreparar { get; set; }
        public string hDuracaoRetirar { get; set; }
        public string hDuracaoRolar { get; set; }
        public string hEntreLotes { get; set; }
        public string hf { get; set; }
        public string hi { get; set; }
        public string hLivrar { get; set; }
        public string hPreparar { get; set; }
        public string hPrevTermino { get; set; }
        public string hRetira { get; set; }
        public string hRolar { get; set; }
        public bool LatchRetira { get; set; }
        public bool LatchRolar { get; set; }
        public int NCheio { get; set; }
        public int NDescarregado { get; set; }
        public int NVagao { get; set; }
        public string Status { get; set; }
        public string hEntreLotesAnt { get; set; }
        public string hPrepararAnt { get; set; }
        public string hRolarAnt { get; set; }
        public string hDuracaoRolarAnt { get; set; }
        public string hiAnt { get; set; }
        public int NVagaoAnt { get; set; }
        public string hfAnt { get; set; }
        public string hDuracaoRetirarAnt { get; set; }
        public string hRetiraAnt { get; set; }
    }
}
