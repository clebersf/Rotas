using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class TagGroupViewModel
    {
        public long Id { get; set; }
        public long? TagId { get; set; }
        public long? ParentId { get; set; }
        public IEnumerable<rTagGroup> rTagGroups { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
        public IEnumerable<rTagGroup> Parents { get; set; }
        public List<string> NameType { get; set; }
        public List<string> NameParent { get; set; }
        [Display(Name = "Nome Opc Server")]
        public string OpcServer { get; set; }
        [Display(Name = "End. Opc Server")]
        public string AddrOpcServer { get; set; }
        [Display(Name = "Url Opc Leitura")]
        public string Url_Read { get; set; }
        [Display(Name = "Url Opc Escrita")]
        public string Url_Write { get; set; }
        public IEnumerable<Tops.Domain.Type> Types { get; set; }
        [Display(Name = "Id")]
        public long rTagGroupId { get; set; }
        [Display(Name = "Sql Procedure")]
        public string SqlProcedure { get; set; }
        public string Name { get; set; }
        [Display(Name = "Serviço Windows")]
        public string WindowsService { get; set; }
        [Display(Name = "Filtro Negado")]
        public string NegFilter { get; set; }
        [Display(Name = "Filtro")]
        public string Filter { get; set; }
        [Display(Name = "Nome da Tag")]
        public string Tag { get; set; }

        [Display(Name = "Grupo / Membro")]
        public string Parent { get; set; }

        [Display(Name = "Pooling")]
        public double Rate { get; set; }

        [Display(Name = "Categoria")]
        public string Type { get; set; }
        [Display(Name = "Apelido")]
        public string Alias { get; set; }

        public long? TypeId { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
    }

    public class rTagGroupList
    {
        public rTagGroup rTagGroup { get; set; }
        public Tops.Domain.Type Type { get; set; }
        public rTagGroup Parent { get; set; }
    }


}