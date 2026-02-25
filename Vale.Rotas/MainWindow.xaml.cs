using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager;

/// <summary>
/// Camada de apresentação do sistema rotas.
/// </summary>

namespace Vale.Rotas
{
    /// <summary>
    /// Janela GUI principal do sistema.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Instancias de objetos da camada de infraestrutura para transações com o banco de dados
        private IrRouteGraphLocationWriteRead RouteWriteRead;
        private ILocationWriteRead LocationWriteRead;
        private IrRouteActiveWriteRead rRouteActiveWriteRead;
        private IrRouteQueueWriteRead rRouteQueueWriteRead;
        private IrRouteReplaceWriteRead rRouteReplaceWriteRead;
        private ILogWriteRead LogWriteRead;
        private Ivw_Route_Consistency_ReadOnly vw_Route_Consistency_ReadOnly;
        private IvwRouteLogReadOnly vwRouteLogReadOnly;
        // Variáveis auxiliares com escopo da Janela GUI principal do sistema.
        private TextBox tbmessage = new TextBox();
        // 💡 CORREÇÃO 1: Inicialize wdQueue como null. REMOVA a inicialização antecipada.
        private Add wdQueue = null;
        private Consistency wdConsist = new Consistency();
        public WComm wcomm { get; set; } = new WComm();
        int IndexTab = 0;
        private DispatcherTimer timer;
        private List<Location> locations = new List<Location>();
        private List<Location> sons = new List<Location>();
        List<Logs> log = new List<Logs>();
        DateTime maxdhlog = DateTime.Now.AddDays(-1);


        // Para desabilitar botão de fechar
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
               
        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint MF_ENABLED = 0x00000000;

        const uint SC_CLOSE = 0xF060;

        const int WM_SHOWWINDOW = 0x00000018;
        const int WM_CLOSE = 0x10;


        IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SHOWWINDOW)
            {
                IntPtr hMenu = GetSystemMenu(hwnd, false);
                if (hMenu != IntPtr.Zero)
                {
                    EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                }
            }
            else if (msg == WM_CLOSE)
            {
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.hwndSourceHook));
            }
        }


        // fim

        /// <summary>
        /// Construtor da Janela GUI principal do sistema.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            pbFooter.IsIndeterminate = true;
            tbmessage.TextChanged += Message_TextChanged;
            cbDischarge.IsChecked = System.Configuration.ConfigurationManager.AppSettings["cbDischarge"] == "True" ? true : false;
            cbBoarding.IsChecked = System.Configuration.ConfigurationManager.AppSettings["cbBoarding"] == "True" ? true : false;
            try
            {
                WriteReadContext ctx = new WriteReadContext();
                ctx.Database.Connection.Open();
                ctx.Database.Connection.Close();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(10000);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            catch(Exception ex)
            {
                //A principal forma de erro é o acesso ao banco de dados. mediante a consulta teste ao banco de dados
                MessageBox.Show("Possível erro de acesso ao banco de dados. Entre em contato com a manutenção. -> " + ex.Message);
                System.Windows.Application.Current.Shutdown();         
            }
            finally 
            {
                
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      long ParentId = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["RouteParentId"]);
                      this.LocationWriteRead = new LocationWriteRead();
                      locations = LocationWriteRead.All().Where(p => (p.TypeId == 56 || p.TypeId == 57) && p.ParentId == ParentId).ToList();
                      sons = (from loc in locations
                                  join son in LocationWriteRead.All() on loc.Id equals son.ParentId
                                  select son).ToList();
                      
                      filter_active();
                      filter_queue();
                      pbFooter.IsIndeterminate = false;
                  }
                ));
            }
        }

        /// <summary>
        /// Ações da thread periodica (10 segundos)
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            // Filtrar janela de fila de rotas
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() =>
                 {
                     if (tabGeneral.SelectedIndex == 1)
                     {
                         pbFooter.IsIndeterminate = true;
                         filter_queue();
                         filter_active();
                         GdActiveRoute_SelectionChanged(this.gdActiveRoute, null);
                         pbFooter.IsIndeterminate = false;
                     }
                 }
               ));
        }

        /// <summary>
        /// Evento de mudança do objeto de texto da tela que informa ao usuário os eventos do sistema (Texto na parte inferir da tela)
        /// </summary>
        private void Message_TextChanged(object sender, TextChangedEventArgs e)
        {
            long parentid = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["RouteParentId"]);
            
            var lstid = ((tbmessage.Text).Split()).ToList();
            var locid = (from  ids in lstid
                      join l in locations.Where(p => p.ParentId == parentid) on ids equals l.Id.ToString() select l.Id).FirstOrDefault();
            if (wcomm.Id == 0)
                wcomm.Id = 214;
            //Registro de logs do sistema
            this.LogWriteRead = new LogWriteRead();
            long appId = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["ApplicationId"]);
            Guid id = Guid.NewGuid();
            if (locid == 0)
                locid = parentid;
            this.LogWriteRead.Save(new Log { Id = id, User = Environment.MachineName, ApplicationId = appId, Message = tbmessage.Text, LocationId = locid, dh = DateTime.Now });
            wcomm.Id = 0;
            // Atualização de filtros da tabela de fila de rotas
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() =>
                 {
                     pbFooter.IsIndeterminate = true;
                     filter_queue();
                     pbFooter.IsIndeterminate = false;
                 }
               ));
            if (tbmessage.Text != "")
            {
                tbFooter.Text = DateTime.Now.ToString() + " - " + tbmessage.Text;
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            pbFooter.IsIndeterminate = true;

            // 1. GARANTIR A INSTÂNCIA E ATRIBUIÇÃO (APENAS UMA VEZ)
            if (wdQueue == null)
            {
                wdQueue = new Add(wcomm);

                // 📢 ATRIBUIÇÃO DO CALLBACK: AQUI ESTÁ CORRETO
                wdQueue.WindowHiddenCallback = TempWindow_Hidden;
                wdQueue.Closed += TempWindow_Closed;
            }

            // 📢 ATUALIZAÇÃO: Atualiza o wcomm da janela Add ANTES de exibi-la.
            // Isso garante que a janela Add use os dados mais recentes da MainWindow.
            wcomm.replace = false;
            wdQueue.wcomm = wcomm;

            // 2. GARANTIR A VISIBILIDADE E O FOCO:
            if (wdQueue.Visibility == Visibility.Hidden || wdQueue.Visibility == Visibility.Collapsed)
            {
                // Removidas as reatribuições desnecessárias!
                wdQueue.Show();
                wdQueue.Activate();
                wdQueue.WindowState = WindowState.Normal;
            }
            else
            {
                // Removidas as reatribuições desnecessárias!
                wdQueue.Activate();
            }

            // Ações de feedback final
            pbFooter.IsIndeterminate = false;

        }

        // ESTE É O NOVO MÉTODO SIMILAR AO TempWindow_Closed
        private void TempWindow_Hidden()
        {
            // Seu código do TempWindow_Closed, adaptado para não usar o (Window)sender
            // e garantindo que é executado na UI Thread.
            string g = "";
            this.filter_queue();
            // Assegurando que a lógica de UI rode na Dispatcher Thread da janela principal
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    // **AÇÕES DE LIMPEZA E ATUALIZAÇÃO**
                    pbFooter.IsIndeterminate = true;
                    tbmessage.Text = wcomm.message;
                    this.filter_active();
                    //this.filter_queue();
                    //this.filter_log();
                    btConsistency.IsEnabled = true;
                    //btAdd.IsEnabled = true;
                    wcomm.replaceId = -1;
                    wcomm.replace = false;
                    pbFooter.IsIndeterminate = false;
                }
            ));

            // Ações que não precisam rodar no Dispatcher (ou já o fazem)
            wcomm.isOpen_Add = false;
            wcomm.isOpen_Consistency = false;

            // Opcional: Aqui você pode colocar a lógica que estava no TempWindow_Closed 
            // para atualizar o objeto wcomm, se necessário, como fazia com:
            // if (wtemp.GetType() == typeof(Add))
            //     wcomm = ((Vale.Rotas.Add)wtemp).wcomm;
            // Se wdQueue ainda é a sua instância, você pode usar:
            // wcomm = wdQueue.wcomm;
        }

        /// <summary>
        /// Evento padrão para fechamento de janelas do sistema
        /// </summary>
        private void TempWindow_Closed(object sender, EventArgs e)
        {
            // Seu código do TempWindow_Closed, adaptado para não usar o (Window)sender
            // e garantindo que é executado na UI Thread.
            string g = "";
            this.filter_queue();
            // Assegurando que a lógica de UI rode na Dispatcher Thread da janela principal
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    // **AÇÕES DE LIMPEZA E ATUALIZAÇÃO**
                    pbFooter.IsIndeterminate = true;
                    tbmessage.Text = wcomm.message;
                    this.filter_active();
                    //this.filter_queue();
                    //this.filter_log();
                    btConsistency.IsEnabled = true;
                    //btAdd.IsEnabled = true;
                    wcomm.replaceId = -1;
                    wcomm.replace = false;
                    pbFooter.IsIndeterminate = false;
                }
            ));

            // Ações que não precisam rodar no Dispatcher (ou já o fazem)
            wcomm.isOpen_Add = false;
            wcomm.isOpen_Consistency = false;

            // Opcional: Aqui você pode colocar a lógica que estava no TempWindow_Closed 
            // para atualizar o objeto wcomm, se necessário, como fazia com:
            // if (wtemp.GetType() == typeof(Add))
            //     wcomm = ((Vale.Rotas.Add)wtemp).wcomm;
            // Se wdQueue ainda é a sua instância, você pode usar:
            // wcomm = wdQueue.wcomm;
        }

        /// <summary>
        /// Filtro para atualização de tabelas de rotas ativas
        /// </summary>
        private void filter_active()
        {
            this.RouteWriteRead = new rRouteGraphLocationWriteRead();
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.rRouteReplaceWriteRead = new rRouteReplaceWriteRead();
            var locs = locations.ToList();
            if (cbBoarding.IsChecked != cbDischarge.IsChecked)
            {
                if (cbBoarding.IsChecked.Value)
                {
                    locs = locations.Where(l => l.Name.Contains("Pier")).ToList();
                }
                else
                {
                    locs = locations.Where(l => !l.Name.Contains("Pier")).ToList();
                }
            }

            
            var active = from act in this.rRouteActiveWriteRead.All()
                         join loc in locs on act.Id equals loc.Id
                         join rot in this.RouteWriteRead.All() on act.Id equals rot.LocationId
                         join rep in this.rRouteReplaceWriteRead.All() on rot.LocationId equals rep.LocationId into repgenerals
                         from repgeneral in repgenerals.DefaultIfEmpty()
                         select new Route { Id = rot.LocationId,  Completa = loc.Name, Resumida = loc.Alias,
                             Substituta = repgeneral != null ? repgeneral.Location.Name : "" };
            //var idx = gdQueueRoute.SelectedIndex;
            //gdQueueRoute.SelectedIndex = idx;
            var obactive = new ObservableCollection<Route>(active);
            pbFooter.IsIndeterminate = false;
            gdActiveRoute.ItemsSource = obactive;
            lblActiveRouteNRow.Content = active.Count().ToString() + " linha(s)";
        }
        /// <summary>
        /// Filtro para atualização de tabelas de rotas na fila
        /// </summary>
        private void filter_queue()
        {
            // consulta filtrada para atualização da tabela
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();
            this.vw_Route_Consistency_ReadOnly = new vw_Route_Consistency_ReadOnly();
            if (gdQueueRoute.SelectedItems.Count > 0)
            {
                btClear.IsEnabled = true;
            }
            else
            {
                btClear.IsEnabled = false;
            }
            
            var cons = this.vw_Route_Consistency_ReadOnly.All();
            var queue = (from rot in this.rRouteQueueWriteRead.All()
                         join c in cons on rot.Id equals c.Id
                         select new Queue
                         {
                             Id = rot.Location.Id,
                             Cod = rot.Id,
                             Completa = rot.Location.Name,
                             Resumida = rot.Location.Alias,
                             Atualização = DateTime.Now,
                             Consistente = c.Consistency == 0 ? "Não" : "Sim"
                         }).ToList();
            if (cbBoarding.IsChecked != cbDischarge.IsChecked)
            {
                if (cbBoarding.IsChecked.Value)
                {
                    queue = queue.Where(p=>p.Completa.Contains("Pier")).ToList();
                }
                else
                {
                    queue = queue.Where(p => !p.Completa.Contains("Pier")).ToList();
                }
            }
            tbmessage.Focus();
            pbFooter.IsIndeterminate = false;
            gdQueueRoute.ItemsSource = queue;
            lblQueueRouteNRow.Content = queue.Count().ToString() + " linha(s)";
        }
        /// <summary>
        /// Filtro para atualização de tabelas de logs do sistema
        /// </summary>
        private void filter_log()
        {
            // consulta filtrada para atualização da tabela
            this.vwRouteLogReadOnly = new vwRouteLogReadOnly();            
            var axlog = this.vwRouteLogReadOnly.All();
            gdLog.ItemsSource = axlog;
            gdLog.Items.SortDescriptions.Clear();
            gdLog.Items.SortDescriptions.Add(new SortDescription("Horário", ListSortDirection.Descending));
            gdLog.Items.Refresh();
            lblLogNRow.Content = axlog.Count().ToString() + " linha(s)";
            pbFooter.IsIndeterminate = false;
        }
        /// <summary>
        /// Botão para remover rotas da fila de rotas
        /// </summary>
        private void BtRemove_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar rota da fila de rotas
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();
            var item = (Queue)gdQueueRoute.SelectedItem;
            this.rRouteQueueWriteRead.Delete(item.Id);
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() =>
                 {
                     pbFooter.IsIndeterminate = true;
                     filter_queue();
                     pbFooter.IsIndeterminate = false;
                 }
               ));
            var bd = (IEnumerable<Queue>)gdQueueRoute.ItemsSource;
            if (!bd.Any())
            {
                btRemove.IsEnabled = false;
            }
            wcomm.routeId = item.Id;
            tbmessage.Text = "A rota de código " + item.Cod + " foi removida da fila de rotas";
            
        }

        /// <summary>
        /// Evento de alteração da gridview de fila de rotas
        /// </summary>
        private void GdQueueRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //atualização da interface mediante a alteração da gridview
            if (gdQueueRoute.SelectedItems.Count > 0)
            {
                btClear.IsEnabled = true;
                btRemove.IsEnabled = true;
                var item = (Queue)gdQueueRoute.SelectedItem;
                if (item.Consistente == "Sim")
                {
                    btOperation.IsEnabled = true;
                }
                else
                {
                    btOperation.IsEnabled = false;
                }
            }
            else
            {
                btClear.IsEnabled = false;
                btRemove.IsEnabled = false;
                btOperation.IsEnabled = false;
            }
        }

        /// <summary>
        /// Botão para iniciar uma operação de rota
        /// </summary>
        private void BtOperation_Click(object sender, RoutedEventArgs e)
        {
            // atulização do banco de dados para inciar operação de uma rota
            this.RouteWriteRead = new rRouteGraphLocationWriteRead();
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();
            
            var item = (Queue)gdQueueRoute.SelectedItem;
            this.rRouteActiveWriteRead.Save(new rRouteActive { Id = item.Id, dh = DateTime.Now });
            this.rRouteQueueWriteRead.Delete(item.Id);

            var Son = this.sons.FirstOrDefault(p => p.ParentId == item.Id);
            if (Son != null)
                this.rRouteActiveWriteRead.Save(new rRouteActive { Id = Son.Id, dh = DateTime.Now });


            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      pbFooter.IsIndeterminate = true;
                      // Consulta das informações de rotas ativas para animar a tabela de rotas ativas
                      filter_active();
                      filter_queue();
                      pbFooter.IsIndeterminate = false;
                  }
                ));
            tabGeneral.SelectedIndex = 0;
            var bd = (IEnumerable<Queue>)gdQueueRoute.ItemsSource;
            if (!bd.Any())
            {
                btOperation.IsEnabled = false;
            }
            wcomm.routeId = item.Id;
            tbmessage.Text = "Iniciada a operação da rota de código " + item.Cod;
        }

        /// <summary>
        /// Botão para limpar a fila de rotas
        /// </summary>
        private void BtClear_Click(object sender, RoutedEventArgs e)
        {
            // limpar a tabela da fila de rota
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();
            this.rRouteQueueWriteRead.DeleteAll();
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() =>
                 {
                     pbFooter.IsIndeterminate = true;
                     filter_queue();
                     pbFooter.IsIndeterminate = false;
                 }
               ));
            tbmessage.Text = "Todas as rota da fila foram removidas da lista";
           
        }

        /// <summary>
        /// Mudança na tabela de rotas ativas
        /// </summary>
        private void GdActiveRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Atualizar interface mediante alteração da griview de rotas ativas
            if (gdActiveRoute.SelectedItems.Count > 0)
            {                
                btFinal.IsEnabled = true;
                var item = (Route)gdActiveRoute.SelectedItem;
                var id = this.RouteWriteRead.All().First(p => p.LocationId == item.Id).Id;
                if (item.Substituta != "")
                {
                    btFinal.Content = "Final./Substituir";
                    btCancelReplace.IsEnabled = true;
                    btReplace.IsEnabled = false;
                }
                else
                {
                    btFinal.Content = "Finalizar";
                    btCancelReplace.IsEnabled = false;
                    btReplace.IsEnabled = true;
                }
            }
            else
            {
                btFinal.IsEnabled = false;
                btReplace.IsEnabled = false;
                btCancelReplace.IsEnabled = false;
            }
        }

        /// <summary>
        /// Botão para finalizar uma rota em operação
        /// </summary>
        private void BtFinal_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar rota da lista de rotas ativas, e em caso de substituição de rota substituir a rota pela rota substituta
            this.RouteWriteRead = new rRouteGraphLocationWriteRead();          
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.rRouteReplaceWriteRead = new rRouteReplaceWriteRead();
           

            var item = (Route)gdActiveRoute.SelectedItem;
            var id = this.RouteWriteRead.All().First(p => p.LocationId == item.Id).LocationId;
            if (this.rRouteReplaceWriteRead.All().Any(p=>p.LocationId == id))
            {                
                var repid = this.rRouteReplaceWriteRead.All().First(p => p.LocationId == id).Id;
                this.rRouteActiveWriteRead.Save(new rRouteActive { Id = repid, dh = DateTime.Now });
                var Sonsub = this.sons.FirstOrDefault(p => p.ParentId == repid);
                if (Sonsub != null)
                    this.rRouteActiveWriteRead.Save(new rRouteActive { Id = Sonsub.Id, dh = DateTime.Now });
                this.rRouteReplaceWriteRead.Delete(repid);
                this.rRouteActiveWriteRead.Delete(id);
                wcomm.routeId = id;
                tbmessage.Text = "Iniciada a operação da rota de código " + repid + " que substituiu a operação da rota código " + id;
            }
            else
            {
                wcomm.routeId = id;
                tbmessage.Text = "Finalizada a operação da rota de código " + id;                
                this.rRouteActiveWriteRead.Delete(id);
            }
            var Son = this.sons.FirstOrDefault(p => p.ParentId == item.Id);
            if (Son != null)
                this.rRouteActiveWriteRead.Delete(Son.Id);

            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      pbFooter.IsIndeterminate = true;
                      // Consulta das informações de rotas ativas para animar a tabela de rotas ativas
                      filter_active();
                      pbFooter.IsIndeterminate = false;
                  }
                ));
            var bd = (IEnumerable<Queue>)gdQueueRoute.ItemsSource;
            if (!bd.Any())
            {
                btOperation.IsEnabled = false;
            }
        }

        /// <summary>
        /// Botão para definir uma rota como substituta de uma rota ativa
        /// </summary>
        /// <summary>
        /// Botão para definir uma rota como substituta de uma rota ativa
        /// </summary>
        private void BtReplace_Click(object sender, RoutedEventArgs e)
        {
            // Lógica de configuração
            this.RouteWriteRead = new rRouteGraphLocationWriteRead();

            // Configurações específicas para substituição
            wcomm.isOpen_Add = true;
            btReplace.IsEnabled = false;
            wcomm.replace = true;
            var item = (Route)gdActiveRoute.SelectedItem;
            var id = this.RouteWriteRead.All().First(p => p.LocationId == item.Id).LocationId;
            wcomm.replaceId = id;

            // Feedback visual
            pbFooter.IsIndeterminate = true;

            // 📢 CORREÇÃO PRINCIPAL: 
            // Em vez de criar um novo thread, chame a lógica de exibição
            // centralizada no BtAdd_Click para garantir que a janela 'wdQueue'
            // seja manipulada no UI Thread correto.

            // Chama BtAdd_Click para lidar com a exibição (Recomendado se o BtAdd_Click 
            // não contiver lógica que atrapalhe o BtReplace)
            // Se o BtAdd_Click for muito complexo ou não deve ser chamado:

            // === Início da Lógica de Exibição (simulando BtAdd_Click) ===

            // 1. GARANTIR A INSTÂNCIA E ATRIBUIÇÃO (APENAS UMA VEZ)
            // Já garantimos que wdQueue é null na inicialização da MainWindow
            if (wdQueue == null)
            {
                wdQueue = new Add(wcomm);
                // ATRIBUIÇÃO DO CALLBACK: ESSENCIAL no UI Thread
                wdQueue.WindowHiddenCallback = TempWindow_Hidden;
                wdQueue.Closed += TempWindow_Closed;
            }

            // ATUALIZAÇÃO: Garante que os dados do replace (wcomm) sejam passados.
            wdQueue.wcomm = wcomm;

            // 2. GARANTIR A VISIBILIDADE E O FOCO (no UI Thread)
            if (wdQueue.Visibility == Visibility.Hidden || wdQueue.Visibility == Visibility.Collapsed)
            {
                wdQueue.Show();
                wdQueue.Activate();
                wdQueue.WindowState = WindowState.Normal;
            }
            else
            {
                wdQueue.Activate();
            }

            // === Fim da Lógica de Exibição ===

            // Ações de feedback final (após Show/Activate)
            pbFooter.IsIndeterminate = false;
        }

        /// <summary>
        /// Cancelar uma substituição de rotas
        /// </summary>
        private void BtCancelReplace_Click(object sender, RoutedEventArgs e)
        {
            this.RouteWriteRead = new rRouteGraphLocationWriteRead();           
            this.rRouteReplaceWriteRead = new rRouteReplaceWriteRead();
            
            var item = (Route)gdActiveRoute.SelectedItem;
            var id = this.RouteWriteRead.All().First(p => p.LocationId == item.Id).LocationId;
            this.rRouteReplaceWriteRead.DeleteQuery(p => p.LocationId == id);
            wcomm.routeId = id;
            tbmessage.Text = "A substituição da rota " + item.Id + " foi cancelada";
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      pbFooter.IsIndeterminate = true;
                      // Consulta das informações de rotas ativas para animar a tabela de rotas ativas
                      filter_active();
                      pbFooter.IsIndeterminate = false;
                  }
                ));
        }

        /// <summary>
        /// Abrir janela que verifica a consistencia das rotas da fila de rotas
        /// </summary>
        private void BtConsistency_Click(object sender, RoutedEventArgs e)
        {
            //Abrir janela que verifica a consistencia das rotas da fila de rotas
            btConsistency.IsEnabled = false;
            wcomm.isOpen_Consistency = true;
            pbFooter.IsIndeterminate = true;
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    // Create and show the Window
                    wdConsist = new Consistency();
                    wdConsist.Closed += TempWindow_Closed;
                    wdConsist.Show();

                    System.Windows.Threading.Dispatcher.Run();
                }
                catch (Exception)
                {
                    //Exceção ignorada
                }
            }));
            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        /// <summary>
        /// Evento de modificação da navegação das tabs do sistema
        /// </summary>
        private void TabGeneral_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //filtrar as tabelas conforme a navegação das tags
            wcomm.tabindex = tabGeneral.SelectedIndex;
            if (IndexTab != tabGeneral.SelectedIndex)
            {
                if (tabGeneral.SelectedIndex == 0)
                {
                    cbBoarding.Visibility = Visibility.Visible;
                    cbDischarge.Visibility = Visibility.Visible;
                    //filter_active();
                }
                else if (tabGeneral.SelectedIndex == 1)
                {
                    cbBoarding.Visibility = Visibility.Visible;
                    cbDischarge.Visibility = Visibility.Visible;
                    //filter_queue();
                }
                else
                {
                    cbBoarding.Visibility = Visibility.Hidden;
                    cbDischarge.Visibility = Visibility.Hidden;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() =>
                      {
                          pbFooter.IsIndeterminate = true;
                          // Consulta das informações de rotas ativas para animar a tabela de rotas ativas
                          filter_log();
                          pbFooter.IsIndeterminate = false;
                      }
                    ));
                }
            }
            IndexTab = tabGeneral.SelectedIndex;
        }

        /// <summary>
        /// Evento de marcação de rotas de embarque
        /// </summary>
        private void cbBoarding_Click(object sender, RoutedEventArgs e)
        {
            Filter_MainWindow();
        }
        /// <summary>
        /// Evento de marcação de rotas de descarga
        /// </summary>
        private void cbDischarge_Click(object sender, RoutedEventArgs e)
        {
            Filter_MainWindow();
        }
        /// <summary>
        /// Filtro de embarque e descarga
        /// </summary>
        private void Filter_MainWindow() 
        {
            // Filtros da rotas conforme string do objeto
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["cbDischarge"].Value = cbDischarge.IsChecked.ToString();
            settings["cbBoarding"].Value = cbBoarding.IsChecked.ToString();
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
              new Action(() =>
              {
                  pbFooter.IsIndeterminate = true;
                  // Consulta das informações de rotas ativas para animar a tabela de rotas ativas
                  filter_active();
                  filter_queue();
                  pbFooter.IsIndeterminate = false;
              }
    ));
        }

        
    }
}
