using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class MaintenanceViewModel
    {
        public string Message { get; set; }
        public int lines { get; set; }
        public List<SelectedGuid> Selecteds { get; set; }
        public List<string> NameLocations { get; set; }
        public Guid Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Data Prevista Inicial")]
        public DateTime dhi_prev { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Data Prevista Final")]
        public DateTime dhf_prev { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Data Real Inicial")]
        public DateTime dhi_real { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Data Real Final")]
        public DateTime dhf_real { get; set; }
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "Concluído")]
        public bool Completed { get; set; }
    }

}