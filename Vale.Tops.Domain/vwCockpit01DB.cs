using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class vwCockpit01DB
    {
        public string Virador { get; set; }
        public string CD_EQP { get; set; }
        public string NOM_EQP { get; set; }
        public string Local_Instalacao_SAPPM { get; set; }
        public string CD_PONTO { get; set; }
        public string NOM_PONTO { get; set; }
        public string CD_Alarme { get; set; }
        public string DS_Alarme { get; set; }
        public int? Criticidade { get; set; }
        public DateTime Data_Ultima_Coleta { get; set; }
    }
}
