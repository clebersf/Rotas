using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;

namespace Vale.Rotas
{
    /// <summary>
    /// Interaction logic for Consistency.xaml
    /// </summary>
    public partial class Consistency : Window
    {
        private Ivw_Route_Consistency_Feeder_ReadOnly vw_Route_Consistency_Feeder_ReadOnly;
        private Ivw_Route_Consistency_Reversal_ReadOnly vw_Route_Consistency_Reversal_ReadOnly;
        private Ivw_Route_Consistency_Tripper_ReadOnly vw_Route_Consistency_Tripper_ReadOnly;
        private Ivw_Route_Consistency_Rule_ReadOnly vw_Route_Consistency_Rule_ReadOnly;
        private Ivw_Route_Consistency_Damper_ReadOnly vw_Route_Consistency_Damper_ReadOnly;
        public Consistency()
        {
            InitializeComponent();
            
            filter();
        }

        private void filter ()
        {
            this.vw_Route_Consistency_Feeder_ReadOnly = new vw_Route_Consistency_Feeder_ReadOnly();
            this.vw_Route_Consistency_Reversal_ReadOnly = new vw_Route_Consistency_Reversal_ReadOnly();
            this.vw_Route_Consistency_Tripper_ReadOnly = new vw_Route_Consistency_Tripper_ReadOnly();
            this.vw_Route_Consistency_Rule_ReadOnly = new vw_Route_Consistency_Rule_ReadOnly();
            this.vw_Route_Consistency_Damper_ReadOnly = new vw_Route_Consistency_Damper_ReadOnly();

            var feeder = from rot in this.vw_Route_Consistency_Feeder_ReadOnly.All().Where(p=>p.BoolVeredict == 0)
                         select new Consist { Completa = rot.Route, Eqp = rot.AssetCurrent, Proximo = rot.AssetNext,
                             Tipo = "Posição de cabeça móvel", Status = "Inconsistente"};

            var damper = from rot in this.vw_Route_Consistency_Damper_ReadOnly.All().Where(p => p.BoolVeredict == 0)
                         select new Consist
                         {
                             Completa = rot.Route,
                             Eqp = rot.AssetCurrent,
                             Proximo = rot.AssetNext,
                             Tipo = "Posição de damper",
                             Status = "Inconsistente"
                         };

            var reversal = from rot in this.vw_Route_Consistency_Reversal_ReadOnly.All().Where(p => p.BoolVeredict == 0)
                           select new Consist
                         {
                             Completa = rot.Route,
                             Eqp = rot.AssetCurrent,
                             Proximo = rot.AssetNext,
                             Tipo = "Sentido de reversão de correia",
                             Status = "Inconsistente"
                         };
            var tripper = from rot in this.vw_Route_Consistency_Tripper_ReadOnly.All().Where(p => p.BoolVeredict == 0)
                          select new Consist
                           {
                               Completa = rot.Route,
                               Eqp = rot.AssetCurrent,
                               Proximo = rot.AssetNext,
                               Tipo = "Posição de tripper",
                               Status = "Inconsistente"
                           };

            var rule = from rot in this.vw_Route_Consistency_Rule_ReadOnly.All().Where(p => p.BoolVeredict == 0)
                        select new Consist
                        {
                            Completa = rot.Route,
                            Eqp = "Rota",
                            Proximo = "",
                            Tipo = "Violação de regras - " + rot.Veredict,
                            Status = "Inconsistente"
                        };

            var gd = reversal.Union(feeder).Union(tripper).Union(rule).Union(damper);
            gdConsistency.ItemsSource = gd;
            lblConsistencyNRow.Content = gd.Count().ToString() + " linhas";
        }
    }
}
