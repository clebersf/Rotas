using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager;

namespace Vale.Rotas
{
    /// <summary>
    /// Interaction logic for Add.xaml
    /// </summary>
    
    public partial class Add : Window
    {
        // Instancias de objetos da camada de infraestrutura para transações com o banco de dados
        private IrRouteGraphLocationWriteRead RouteWriteRead;
        private IrRouteActiveWriteRead rRouteActiveWriteRead;
        private IrRouteQueueWriteRead rRouteQueueWriteRead;
        private IrRouteReplaceWriteRead rRouteReplaceWriteRead;


        public WComm wcomm { get; set; }
        string Destination { get; set; }
        string strFilter { get; set; }
        // 1. Defina um campo Action para o callback
        public Action WindowHiddenCallback { get; set; }
        /// <summary>
        /// Construtor da Janela GUI de adição de rotas do sistema.
        /// </summary>
        public Add(WComm wcomm)
        {
            InitializeComponent();
            strFilter = "";
            this.wcomm = wcomm;
            // Garante que o manipulador de evento está conectado
            this.Closing += Add_Closing;
            this.RouteWriteRead = new rRouteGraphLocationWriteRead();
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.rRouteReplaceWriteRead = new rRouteReplaceWriteRead();
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();

            Destination = System.Configuration.ConfigurationManager.AppSettings["txtRadioButton"];

            if (Destination == System.Configuration.ConfigurationManager.AppSettings["Filter1"])
                rbPier1.IsChecked = true;
            else if (Destination == System.Configuration.ConfigurationManager.AppSettings["Filter2"])
                rbPier2.IsChecked = true;
            else if (Destination == System.Configuration.ConfigurationManager.AppSettings["Filter3"])
                rbManLimp.IsChecked = true;
            else
                rbEmpilhar.IsChecked = true;

            //if (wcomm.replace)
            //    btAdd.Content = "Substituir";
            //else
            //    btAdd.Content = "Adicionar";

            txtAsset1.Text = System.Configuration.ConfigurationManager.AppSettings["txtAsset1"];
            strFilter = txtAsset1.Text;
            filter();
        }
        // 🎯 ESTE É O MÉTODO QUE SERÁ EXECUTADO SEMPRE QUE A JANELA FICAR ATIVA
        private void Add_Activated(object sender, EventArgs e)
        {
            // Coloque aqui toda a lógica que você precisa
            // que seja executada toda vez que a janela for trazida para o primeiro plano.

            this.wcomm = wcomm;
            filter();
        }

        private void Add_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 1. Impedir o fechamento real da janela
            e.Cancel = true;

            // 2. Esconder a janela em vez de fechar
            this.Hide();

            // Opcional: Para feedback visual, você pode querer
            // minimizar a janela para a barra de tarefas
            this.WindowState = WindowState.Minimized;

            // 2. CHAMA O MÉTODO DA JANELA PRINCIPAL
            // Verifica se a Action foi definida e a executa.
            WindowHiddenCallback?.Invoke();
        }

        /// <summary>
        /// Evento do textbox para filtro das rotas a serem adicionadas na fila ou para substituição
        /// </summary>
        private void TxtAsset1_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Filtros da rotas conforme string do objeto
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["txtAsset1"].Value = txtAsset1.Text;
            strFilter = txtAsset1.Text;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            Dispatcher.CurrentDispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      filter();
                  }
                ));
        }

        /// <summary>
        /// Filtro das rotas a serem adicionadas na fila ou para substituição
        /// </summary>
        private void filter ()
        {
            
            // Critérios de filtros para as rotas
            var route = this.RouteWriteRead.All().ToList();
            List<rRouteGraphLocation> except = new List<rRouteGraphLocation>();
            if (Destination == System.Configuration.ConfigurationManager.AppSettings["Filter1"])
            {
                except = route.Where(p => p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter2"]) || p.Location.Name.Contains("CN0") ||
                p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter3"])).ToList();
                route = route.Where(p=>p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter1"])).Except(except).ToList();
            }
            else if (Destination == System.Configuration.ConfigurationManager.AppSettings["Filter2"])
            {
                except = route.Where(p => p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter1"]) || p.Location.Name.Contains("CN0") ||
                p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter3"])).ToList();
                route = route.Where(p => p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter2"])).Except(except).ToList();
            }
            else if (Destination == System.Configuration.ConfigurationManager.AppSettings["Filter3"])
            {
                except = route.Where(p => p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter1"]) || p.Location.Name.Contains("CN0") ||
                p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter2"])).ToList();
                route = route.Where(p => p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter3"])).Except(except).ToList();
            }
            else
            {
                except = route.Where(p => p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter1"]) || p.Location.Name.Contains("CN0") ||
                p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter2"]) ||
                p.Location.Name.Contains(System.Configuration.ConfigurationManager.AppSettings["Filter3"])).ToList();
                route = route.Except(except).ToList();
            }

            // Critérios de filtros para as rotas na fila
            var queue = from qu in this.rRouteQueueWriteRead.All()
                        join rot in route on qu.Id equals rot.LocationId
                        select rot;

            // Critérios de filtros para as rotas
            var active = from qu in this.rRouteActiveWriteRead.All()
                        join rot in route
                        on qu.Id equals rot.LocationId
                        select rot;
            // Critérios de filtros para as rotas
            var words = ((strFilter).Split()).ToList();

            // Casos para rotas substitutas
            if (wcomm.replace)
            {

                queue = Enumerable.Empty<Tops.Domain.rRouteGraphLocation>();
                 
                ReadOnlyContext ctx = new ReadOnlyContext();
                var consistency = ctx.fn_RouteReplacer_I_Route_O_Route(wcomm.replaceId);

                route = (from cons in consistency.Where(p=>p.Consistency == 1)
                        join r in route on cons.Id equals r.LocationId
                        select r).ToList();
            }
            var bd = route.Where(o => words.All(p => o.Location.Name.Contains(p.ToUpper())));

            bd = bd.Except(queue).Except(active).Except(except);

            var source = from q in bd
                    select new Route {Id = q.LocationId, Completa = q.Location.Name, Resumida = q.Location.Alias};

            //gdQueue.ItemsSource = source;
            //lblQueueNRow.Content = source.Count().ToString() + " linhas";

            var obactive = new ObservableCollection<Route>(source);
            gdQueue.ItemsSource = obactive;
            lblQueueNRow.Content = source.Count().ToString() + " linhas";
        }

        /// <summary>
        /// Evento de mudanaça da gridview da fila de rotas
        /// </summary>
        private void GdQueue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // atualização de interface mediante a alteração da tabela
            if (gdQueue.SelectedItems.Count > 0)
            {
                btAdd.IsEnabled = true;
                lblConsistency.Content = "";
            }
            else
            {
                btAdd.IsEnabled = false;
                lblConsistency.Content = "";
            }            
        }

        /// <summary>
        /// Adicionar uma rota a fila de rotas
        /// </summary>
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();
            this.rRouteReplaceWriteRead = new rRouteReplaceWriteRead();
            if (wcomm.replace)
            {                
                var item = (Route)gdQueue.SelectedItem;
                var id = this.RouteWriteRead.All().First(p => p.LocationId == item.Id).LocationId;
                this.rRouteReplaceWriteRead.Save(new rRouteReplace { Id = id, dh = DateTime.Now, LocationId = wcomm.replaceId });
                btAdd.IsEnabled = true;
                wcomm.routeId = id;
                wcomm.message = "A rota de código " + wcomm.replaceId + " teve a rota código " + item.Id + " definida como sua substituta";
                //wcomm.replace = false;
                this.Hide();
                WindowHiddenCallback?.Invoke();
            }
            else
            {                
                var item = (Route)gdQueue.SelectedItem;
                var id = this.RouteWriteRead.All().First(p => p.LocationId == item.Id).LocationId;
                var queue = new rRouteQueue { Id = id, dh = DateTime.Now };
                this.rRouteQueueWriteRead.Save(queue);
                btAdd.IsEnabled = true;
                wcomm.routeId = id;
                wcomm.message = "Adicionado a rota de código " + item.Id + " na fila de rotas.";
                this.Hide();
                WindowHiddenCallback?.Invoke();
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (rbPier1.IsChecked == true)
            {
                Destination = rbPier1.Content.ToString();
            }
            else if (rbPier2.IsChecked == true) { 
                Destination = rbPier2.Content.ToString();
            }
            else if (rbManLimp.IsChecked == true)
            {
                Destination = rbManLimp.Content.ToString();
            }
            else { 
                Destination = "";
            }

            // Filtros da rotas conforme string do objeto
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["txtRadioButton"].Value = Destination;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

            if (wcomm.tabindex == 1 || (wcomm.tabindex == 0 && wcomm.replace))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      filter();
                  }
                ));
            }
        }
    }
}
