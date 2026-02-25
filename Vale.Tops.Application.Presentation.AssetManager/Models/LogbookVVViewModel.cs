using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class LogbookVVViewModel : LogbookViewModel
    {
        
        [Display(Name = "Mat. Op. 1")]
        public virtual string In_Diario_NomeOperador1 { get; set; }        

        [Display(Name = "Data/Hora Op. 1")]
        public virtual int s_Diario_dh_Operador1 { get; set; }
        public virtual int In_Diario_Operador1Hora { get; set; }
        public virtual int In_Diario_Operador1Minuto { get; set; }

        [Display(Name = "Quantidade Vagões")]
        public virtual int In_Diario_QuantidadeVagoes { get; set; }

        [Display(Name = "Mat. Op. 2")]
        public virtual string In_Diario_NomeOperador2 { get; set; }

        [Display(Name = "Data/Hora Op. 2")]
        public virtual int s_Diario_dh_Operador2 { get; set; }
        public virtual int In_Diario_Operador2Hora { get; set; }
        public virtual int In_Diario_Operador2Minuto { get; set; }

        [Display(Name = "Qnd Vagões Op. 1")]
        public virtual int In_Diario_Operador1QtVagoes { get; set; }

        [Display(Name = "Observação")]
        public virtual string In_Diario_Observacao { get; set; }
        [Display(Name = "Obs. Taxa")]
        public virtual string In_Diario_ObservacaoTaxa { get; set; }
        [Display(Name = "VV. Restrito")]
        public virtual bool In_Diario_VVRestrito { get; set; }
        [Display(Name = "Rota. Restrito")]
        public virtual bool In_Diario_RotaRestrita { get; set; }
        [Display(Name = "Qualidade")]
        public virtual bool In_Diario_QualidadeMaterial { get; set; }
        [Display(Name = "Partiu Ciclo")]
        public virtual string s_Diario_PartirCiclo { get; set; }
        public virtual int In_Diario_PartirCicloHora { get; set; }
        public virtual int In_Diario_PartirCicloMinuto { get; set; }
        [Display(Name = "Terminou Descarga")]
        public virtual string s_Diario_PartirTerminoDescarga { get; set; }
        public virtual int In_Diario_TerminoDescargaHora { get; set; }
        public virtual int In_Diario_TerminoDescargaMinuto { get; set; }
        [Display(Name = "Balisa")]
        public virtual string bal { get; set; }
        public virtual int In_Diario_BalizaMenor { get; set; }
        public virtual int In_Diario_BalizaMaior { get; set; }
        
        [Display(Name = "Prefixo")]
        public virtual string In_Diario_Prefixo { get; set; }
        [Display(Name = "Material")]
        public virtual string In_Diario_Material { get; set; }
        [Display(Name = "Area")]
        public virtual string In_Diario_Area { get; set; }
        [Display(Name = "Destino")]
        public virtual string In_Diario_Destino { get; set; }
        

        [Display(Name = "Virador de Vagões")]
        public string Asset { get; set; }

        public IEnumerable<rLogbookVV> Logbookvvs { get; set; }
    }

}