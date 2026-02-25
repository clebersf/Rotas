using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class LocationViewModel
    {
        public long Id { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public string Message { get; set; }
        public int lines { get; set; }
        public IEnumerable<Location> Parents { get; set; }
        public List<string> NameType { get; set; }
        public List<string> NameParent { get; set; }
        public IEnumerable<Tops.Domain.Type> Types { get; set; }
        [Display(Name = "Id")]
        public long LocationId { get; set; }
        public long? ParentId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "Filtro Negado")]
        public string NegFilter { get; set; }
        [Display(Name = "Filtro")]
        public string Filter { get; set; }
        [Display(Name = "Categoria")]
        public string Type { get; set; }
        [Display(Name = "Apelido")]
        public string Alias { get; set; }
        [Display(Name = "Local Pai")]
        public string Parent { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        public long? TypeId { get; set; }
        public List<SelectedInt> Selecteds { get; set; }
    }

    public class LocationList
    {
        public Location Location { get; set; }
        public Tops.Domain.Type Type { get; set; }
        public Location Parent { get; set; }
    }


    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
    }

}