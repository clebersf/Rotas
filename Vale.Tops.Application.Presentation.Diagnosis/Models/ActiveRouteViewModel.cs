// Exemplo: Vale.Tops.Web.Mvc/ViewModels/ActiveRouteViewModel.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace Vale.Tops.Web.Mvc.ViewModels
{
    public class ActiveRouteViewModel
    {
        [Display(Name = "ID da Rota")]
        public long Id { get; set; }

        [Display(Name = "Nome da Rota")]
        // Este campo virá de Location.Resource ou de outra propriedade de descrição
        public string Name { get; set; }

        [Display(Name = "Data de Ativação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime ActivationDate { get; set; }
    }
}