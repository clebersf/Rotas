using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class LogbookEPEEViewModel : LogbookViewModel
    {
        [Display(Name = "Balisa")]
        public virtual string bal { get; set; }

        [Display(Name = "Bal lote")]
        public virtual string bal_lote { get; set; }

        [Display(Name = "Hora op. 1")]
        public virtual string dh_op_1 { get; set; }        

        [Display(Name = "Hora op. 2")]
        public virtual string dh_op_2 { get; set; }        

        [Display(Name = "Hora p0")]
        public virtual string dh_p0 { get; set; }

        [Display(Name = "Hora acop")]
        public virtual string dh_acop { get; set; }

        [Display(Name = "Hora usina")]
        public virtual string dh_usi { get; set; }

        [Display(Name = "Origem 1")]
        public virtual string In_Diario_Origem1 { get; set; }

        [Display(Name = "Matricula 1")]
        public virtual string In_Diario_Matricula { get; set; }

        [Display(Name = "Matricula 2")]
        public virtual string In_Diario_Material1 { get; set; }

        

        [Display(Name = "Nº lote")]
        public virtual int In_Diario_Numero_Lote { get; set; }

        [Display(Name = "Qnd Vagões")]
        public virtual int In_Diario_Quantidade_Vagoes1 { get; set; }

        [Display(Name = "Pref. lote")]
        public virtual int In_Diario_Prefixo_Lote1 { get; set; }

        [Display(Name = "Passadas")]
        public virtual int In_Diario_Passadas { get; set; }

        [Display(Name = "Bal. inicial")]
        public virtual int In_Diario_Baliza_Inicial { get; set; }

        [Display(Name = "Bal. final")]
        public virtual int In_Diario_Baliza_Final { get; set; }

        [Display(Name = "Bal. inicial lote")]
        public virtual int In_Diario_Baliza_Inicial_Lote { get; set; }

        [Display(Name = "Bal. final lote")]
        public virtual int In_Diario_Baliza_Final_Lote { get; set; }

        [Display(Name = "Hora inicio 1")]
        public virtual int In_Diario_Hora_Inicial1 { get; set; }

        [Display(Name = "Minuto inicio 1")]
        public virtual int In_Diario_Minuto_Inicial1 { get; set; }

        [Display(Name = "Hora fim 1")]
        public virtual int In_Diario_Hora_Final1 { get; set; }

        [Display(Name = "Minuto fim 1")]
        public virtual int In_Diario_Minuto_Final1 { get; set; }

        [Display(Name = "Paralização")]
        public virtual bool In_Diario_Paralisacao { get; set; }

        [Display(Name = "Fech. pilha")]
        public virtual bool In_Diario_Fechamento_de_Pilha { get; set; }

        [Display(Name = "Tremonha")]
        public virtual bool In_Diario_Tremonha { get; set; }

        [Display(Name = "Quallidade material")]
        public virtual bool In_Diario_Qualidade_Material_Ok { get; set; }

        [Display(Name = "Rota restrita")]
        public virtual bool In_Diario_Rota_Restrita_Ok { get; set; }

        [Display(Name = "Qualidade nok")]
        public virtual bool In_Diario_Qualidade_Material_Nok { get; set; }
        //public virtual int In_Diario_Restricao { get; set; }

        [Display(Name = "Rota restrita nok")]
        public virtual bool In_Diario_Rota_Restrita_Nok { get; set; }



        [Display(Name = "Obs. paralização")]
        public virtual string In_Diario_Observacao_Paralisacao { get; set; }

        [Display(Name = "Obs. taxa")]
        public virtual string In_Diario_Observacao_Taxa { get; set; }
       
        [Display(Name = "Cod. operação")]
        public virtual string In_Diario_Codigo_Operacao { get; set; }

        [Display(Name = "Area")]
        public virtual string In_Diario_Area { get; set; }

        [Display(Name = "Empilhamento")]
        public virtual string In_Diario_Empilhamento { get; set; }

        [Display(Name = "Insumo")]
        public virtual string In_Diario_Insumo { get; set; }

        [Display(Name = "Hora comun. p0")]
        public virtual int In_Diario_Hora_Comunicacao_P0 { get; set; }

        [Display(Name = "Minuto comun. p0")]
        public virtual int In_Diario_Minuto_Comunicacao_P0 { get; set; }

        [Display(Name = "Hora comun. acop.")]
        public virtual int In_Diario_Hora_Acoplado { get; set; }

        [Display(Name = "Minuto comun. p0")]
        public virtual int In_Diario_Minuto_Acoplado { get; set; }

        [Display(Name = "Hora rodou usina")]
        public virtual int In_Diario_Hora_Rodou_Usina { get; set; }

        [Display(Name = "Minuto rodou usina")]
        public virtual int In_Diario_Minuto_Rodou_Usina { get; set; }

        [Display(Name = "Material 2")]
        public virtual string In_Diario_Material2 { get; set; }

        [Display(Name = "Origem 2")]
        public virtual string In_Diario_Origem2 { get; set; }

        [Display(Name = "Taxa 1")]
        public virtual int In_Diario_Taxa_1 { get; set; }

        [Display(Name = "Taxa 2")]
        public virtual int In_Diario_Taxa_2 { get; set; }

        [Display(Name = "Prefixo lote 2")]
        public virtual int In_Diario_Prefixo_Lote2 { get; set; }

        [Display(Name = "Qnd vagões 2")]
        public virtual int In_Diario_Quantidade_Vagoes2 { get; set; }

        [Display(Name = "Hora inicial 2")]
        public virtual int In_Diario_Hora_Inicial2 { get; set; }

        [Display(Name = "Minuto inicial 2")]
        public virtual int In_Diario_Minuto_Inicial2 { get; set; }

        [Display(Name = "Hora final 2")]
        public virtual int In_Diario_Hora_Final2 { get; set; }

        [Display(Name = "Minuto final 2")]
        public virtual int In_Diario_Minuto_Final2 { get; set; }

        [Display(Name = "Restr. Qual. Mat.")]
        public virtual int In_Diario_Restricao_Qualidade_Material { get; set; }

        [Display(Name = "Restr. Rota.")]
        public virtual int In_Diario_Restricao_Rota_Restrita { get; set; }

        [Display(Name = "Peso lote 1")]
        public virtual int In_Diario_Peso_Lote1 { get; set; }

        [Display(Name = "Peso lote 2")]
        public virtual int In_Diario_Peso_Lote2 { get; set; }




        [Display(Name = "Empilhadeira")]
        public string Asset { get; set; }

        public IEnumerable<rLogbookEPEE> Logbookepees { get; set; }


    }
}