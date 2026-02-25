using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class LogbookViewModel
    {
        [Display(Name = "Ação")]
        public string Action { get; set; }
        public bool isChange { get; set; }
        public IEnumerable<History> Historys { get; set; }
        [Display(Name = "Sim")]
        public int? Yes { get; set; }

        [Display(Name = "Não")]
        public int? No { get; set; }

        public int? NA { get; set; }

        [Display(Name = "Data Registro")]
        public DateTime? dh { get; set; }

        [Display(Name = "Usuário")]
        public string User { get; set; }

        [Display(Name = "Área")]
        public string Area { get; set; }

        [Display(Name = "Observação")]
        public string Observation { get; set; }

        public Guid Id { get; set; }

        [Display(Name = "Baliza Inicial")]
        public int? LandmarkInitial { get; set; }

        [Display(Name = "Baliza Final")]
        public int? LandmarkFinal { get; set; }

        //[Display(Name = "Carga Recuperadora")]
        //public int WeightAsset { get; set; }

        //[Display(Name = "Carga Destino")]
        //public int WeightConjunct { get; set; }

        //[Display(Name = "Berço")]
        //public int ShipCradle { get; set; }

        //[Display(Name = "Porão")]
        //public int Hold { get; set; }

        [Display(Name = "Material")]
        public string Material { get; set; }

        //[Display(Name = "Carga Porão")]
        //public int Quantity { get; set; }

        //[Display(Name = "Taxa Pedida")]
        //public int FlowMeasured { get; set; }

        //[Display(Name = "Taxa Real")]
        //public int FlowReal { get; set; }

        //[Display(Name = "Defeito")]
        //public bool isFail { get; set; }

        [Display(Name = "Hora Inicial")]
        [Range(0, 23)]
        public int? hi { get; set; }

        [Display(Name = "Minuto Inicial")]
        //[Range(0, 59)]
        public int? mi { get; set; }

        [Display(Name = "Hora Final")]
        //[Range(0, 23)]
        public int? hf { get; set; }

        [Display(Name = "Minuto Final")]
        //[Range(0,59)]
        public int? mf { get; set; }

        //public string AreaSp1 { get; set; }

        //public string AreaSp2 { get; set; }

        //[Display(Name = "Recuperadora")]
        //public string Asset { get; set; }

        //[Display(Name = "Destino")]
        //public string Conjunct { get; set; }

        [Display(Name = "Baliza")]
        public string Landmark { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Inicio")]
        public DateTime? dhpi { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pesquisa Fim")]
        public DateTime? dhpf { get; set; }

        public IEnumerable<Logbook> Logbooks { get; set; }

        public IEnumerable<Location> Locations { get; set; }

        public string Message { get; set; }

        public int? lines { get; set; }

        public long? LocationId { get; set; }

        public List<SelectedInt> Selecteds { get; set; }

        //Empilhadeiras
        /*[Display(Name = "Qualidade do Material")]
        public bool MaterialQuality { get; set; }

        [Display(Name = "Rota Restrita")]
        public bool RestrictRoute { get; set; }

        [Display(Name = "Final da Pilha")]
        public bool StackEnd { get; set; }

        [Display(Name = "Baliza Inicial Sentido Norte")]
        public bool LmInitialNorthDirection { get; set; }

        [Display(Name = "Baliza Final Sentido Norte")]
        public bool LmFinalNorthDirection { get; set; }

        [Display(Name = "Tremonha")]
        public bool IsHopper { get; set; }

        [Display(Name = "Número do Lote")]
        public int LotNumber { get; set; }

        [Display(Name = "Número de Vagões")]
        public int NumberOfWagons { get; set; }

        [Display(Name = "Prefixo do Lote")]
        public int LotPrefix { get; set; }

        [Display(Name = "Número de Passadas")]
        public int Steps { get; set; }
        
        public int LotSLandmarkInitial { get; set; }
        
        public int LotSLandmarkFinal { get; set; }
    
        [Display(Name ="Baliza do Lote")]
        public string LotSLandmark { get; set; }

        public int hP0 { get; set; }

        public int mP0 { get; set; }

        public int hUsina { get; set; }

        public int mUsina { get; set; }

        public int hAcoplado { get; set; }

        public int mAcoplado { get; set; }

        [Display(Name ="Observação de Taxa")]
        public string FlowObservation { get; set; }

        [Display(Name ="Forma de Empilhamento")]
        public string StackingWay { get; set; }

        [Display(Name ="Fora de Pilha")]
        public string OutOfStack { get; set; }*/

        public string IndexPage { get; set; }
    }

}