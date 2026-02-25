//using Siemens.Opc.Da;
//using System;
//using System.Threading;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.ServiceProcess;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Siemens.Opc;
//using Vale.Tops.Domain;
//using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
//using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
//using System.Collections.Specialized;
//using System.Configuration;
//using System.IO;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using System.CodeDom;

//namespace Vale.Tops.Integration.Infrastructure.OpcClientService
//{
//    public partial class Service : ServiceBase
//    {


//        #region Private Members
//        private Server m_Server = new Server();
//        private Subscription m_Subscription = null;
//        private List<Subscription> m_Subscriptions = new List<Subscription>();
//        private List<string> plcs = new List<string>();
//        private List<Tag> itens = new List<Tag>();

//        private ILogWriteRead LogWriteRead;
//        private IrInstrumentMeasureWriteRead rInstrumentMeasureWriteRead;
//        private IrTagWriteWriteRead rTagWriteWriteRead;
//        private Ivw_Opc_TagGroup_ReadOnly vw_Opc_TagGroup_ReadOnly;
//        private string txtServerUrlText = "";
//        private System.Timers.Timer _Timer_Write;
//        private System.Timers.Timer _Timer_Bd;

//        private int ParentId = 0;
//        private int npool = 0;
//        private int ApplicationId = 0;
//        private int istolog = 0;
//        private int iswrite = 0;
//        private DateTime lastdh = DateTime.Now;


//        private List<rInstrumentMeasure> meas = new List<rInstrumentMeasure>();
//        private List<vw_Opc_TagGroup> tags = new List<vw_Opc_TagGroup>();
//        #endregion

//        #region Construction

//        public Service()
//        {
//            InitializeComponent();

//            try
//            {
//                // Configuração leitura e escrita banco de dados
//                this.vw_Opc_TagGroup_ReadOnly = new vw_Opc_TagGroup_ReadOnly();
//                this.rInstrumentMeasureWriteRead = new rInstrumentMeasureWriteRead();
//                this.rTagWriteWriteRead = new rTagWriteWriteRead();
//                this.LogWriteRead = new LogWriteRead();
//                // set the sever we want to connet to
//                ParentId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ParentId"]);
//                istolog = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IsToLog"]);
//                iswrite = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IsWrite"]);
//                var ServerUrl = this.vw_Opc_TagGroup_ReadOnly.All().Where(p => p.ParentId == ParentId);
//                txtServerUrlText = ServerUrl.First().ServerUrl;
//                ApplicationId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ApplicationId"]);

//                tags = this.vw_Opc_TagGroup_ReadOnly.All().Where(p => p.ParentId == ParentId).ToList();
//                plcs = tags.Select(o => o.PLC).Distinct().ToList();
//                // Consulta tabela de tags/valores
//                meas = (from rmeas in this.rInstrumentMeasureWriteRead.All()
//                        join tg in tags on rmeas.TagId equals tg.TagId
//                        select rmeas).ToList();



//                this._Timer_Write = new System.Timers.Timer();
//                this._Timer_Write.Elapsed += new
//                System.Timers.ElapsedEventHandler(timer_Tick_Write);
//                this._Timer_Write.Enabled = false;
//                this._Timer_Write.Interval = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]);

//                this._Timer_Bd = new System.Timers.Timer();
//                this._Timer_Bd.Elapsed += new
//                System.Timers.ElapsedEventHandler(timer_Tick_Bd);
//                this._Timer_Bd.Enabled = false;
//                this._Timer_Bd.Interval = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]);

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Falha ao iniciar o serviço. - " + ex.InnerException);
//            }

//        }

//        #endregion

//        #region Service methods
//        protected override void OnStart(string[] args)
//        {
//            try
//            {
//                OnConnect();
//                startMonitorItems(tags);
//                this._Timer_Write.Enabled = true;
//                this._Timer_Bd.Enabled = true;
//            }
//            catch (Exception ex)
//            {
//                this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "On Start Service " + ex.Message });
//            }
//            finally
//            {
//                this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Service Opc Client started" });
//            }
//        }

//        protected override void OnStop()
//        {
//            stopMonitorItems();
//            OnDisconnect();
//            ShutDownRequest("");
//            this._Timer_Write.Enabled = false;
//            this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Service Opc Client ended" });
//        }

//        private void timer_Tick_Bd(object sender,
//      System.Timers.ElapsedEventArgs e)
//        {
//            try
//            {
//                Thread tl = new Thread(NewThreadTagsBulk(meas));
//            }
//            catch (Exception exception)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Config e start write " + exception.Message });
//            }
//        }


//        private void timer_Tick_Write(object sender,
//          System.Timers.ElapsedEventArgs e)
//        {
//            try
//            {
//                if (iswrite == 1)
//                {
//                    WriteTags();
//                }
//            }
//            catch (Exception exception)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Config e start write " + exception.Message });
//            }
//        }

//        #endregion


//        #region Connect and Disconnect Server
//        /// <summary>
//        /// Handles connect procedure
//        /// </summary>
//        private void OnConnect()
//        {
//            if (m_Server == null)
//            {
//                // Create a server object
//                m_Server = new Server();
//            }

//            try
//            {
//                // connect to the server
//                m_Server.Connect(txtServerUrlText);
//            }
//            catch (Exception exception)
//            {
//                // Cleanup
//                m_Server = null;
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Connect failed" + exception.Message });
//            }

//        }

//        /// <summary>
//        /// Handles disconnect procedure
//        /// </summary>
//        private void OnDisconnect()
//        {
//            if (m_Server == null)
//            {
//                return;
//            }

//            try
//            {
//                // Disconnect
//                m_Server.Disconnect();
//                m_Server.Dispose();
//                m_Server = null;
//            }
//            catch (Exception exception)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Disconnect failed" + exception.Message });
//            }
//        }

//        #endregion

//        #region Tags iteraction

//        private void WriteTags()
//        {
//            try
//            {
//                this.vw_Opc_TagGroup_ReadOnly = new vw_Opc_TagGroup_ReadOnly();
//                this.rTagWriteWriteRead = new rTagWriteWriteRead();
//                var wtags = this.vw_Opc_TagGroup_ReadOnly.All().Where(p => p.ParentId == ParentId).ToList();
//                var wgates = this.rTagWriteWriteRead.All().Where(p => p.Write == true);
//                var wtaggate = (from wgate in wgates
//                                join wtag in wtags on wgate.Id equals wtag.TagId
//                                select new { Tag = wtag.Tag, Value = wgate.Value }).ToList();
//                var update = (from wgate in wgates
//                              join wtag in wtags on wgate.Id equals wtag.TagId
//                              select new rTagWrite { Id = wgate.Id, Tag = wgate.Tag, Value = wgate.Value, Write = false }).ToList();
//                Thread tl = new Thread(NewThreadWriteTagBulk(update));
//                foreach (var unit in wtaggate)
//                {
//                    try
//                    {
//                        m_Server.Write(unit.Tag, unit.Value);
//                    }
//                    catch
//                    {
//                        if (istolog == 1)
//                            this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Write tag - " + unit.Tag });
//                    }
//                }

//            }
//            catch (Exception exception)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Write tags general- " + exception.Message });
//            }
//        }

//        #endregion

//        #region Internal Helper Methods
//        void startMonitorItems(List<vw_Opc_TagGroup> tags)
//        {
//            // Check if we have a subscription. If not - create a new subscription.
//            try
//            {
//                foreach (string s in plcs)
//                {
//                    // Create subscription
//                    var sub = m_Server.CreateSubscription(s, OnDataChange);
//                    m_Subscriptions.Add(sub);
//                    foreach (var tag in tags.Where(p => p.PLC == s).ToList())
//                    {
//                        sub.AddItem(tag.Tag, ((int)tag.TagId));
//                    }
//                }
//            }
//            catch (Exception exception)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Start Monitor - " + exception.Message });
//                return;
//            }
//        }

//        void CheckMonitorItems(List<vw_Opc_TagGroup> tags)
//        {
//            // Check if we have a subscription. If not - create a new subscription.
//            try
//            {
//                foreach (string s in plcs)
//                {
//                    // Create subscription
//                    var sub = m_Server.CreateSubscription(s, OnDataChange);
//                    m_Subscriptions.Add(sub);
//                    foreach (var tag in tags.Where(p => p.PLC == s).ToList())
//                    {
//                        sub.AddItem(tag.Tag, ((int)tag.TagId));
//                    }
//                }
//            }
//            catch (Exception exception)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Start Monitor - " + exception.Message });
//                return;
//            }
//        }

//        void stopMonitorItems()
//        {
//            try
//            {
//                int i = 0;
//                foreach (string s in plcs)
//                {
//                    foreach (var tag in tags.Where(p => p.PLC == s).ToList())
//                    {
//                        m_Subscriptions[i].RemoveItem(tag.Tag);
//                    }
//                    i++;
//                }
//                foreach (Subscription sub in m_Subscriptions)
//                {
//                    m_Server.DeleteSubscription(sub);
//                    m_Subscriptions.Remove(sub);
//                    m_Subscriptions.Clear();
//                }
//            }
//            catch (Exception ex)
//            {
//                if (istolog == 1)
//                    this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Stop Monitor - " + ex.Message });
//                return;
//            }
//        }

//        #endregion

//        #region Event Handlers
//        /// <summary>
//        /// Show shutdown message
//        /// When receiving a shutdown message just disconnect.
//        /// </summary>
//        public void ShutDownRequest(string reason)
//        {
//            OnDisconnect();
//        }

//        /// <summary>
//        /// callback to receive datachanges
//        /// </summary>
//        /// <param name="clientHandle"></param>
//        /// <param name="value"></param>
//        private void OnDataChange(IList<DataValue> DataValues)
//        {
//            try
//            {
//                foreach (DataValue value in DataValues)
//                {
//                    TimeSpan ts = DateTime.Now.Subtract(meas.Where(p => p.TagId == value.ClientHandle).First().dh);
//                    if (ts.Seconds * 1000 + ts.Milliseconds > Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]))
//                    {
//                        if (value.Error != 0)
//                        {
//                            foreach (var unit in meas.Where(p => p.TagId == value.ClientHandle))
//                            {
//                                unit.ErrorCode = value.Error.ToString();
//                                unit.ErrorString = value.Quality.ToString();
//                                unit.dh = DateTime.Now;
//                            }
//                        }
//                        else
//                        {
//                            foreach (var unit in meas.Where(p => p.TagId == value.ClientHandle))
//                            {
//                                unit.ErrorCode = "";
//                                unit.ErrorString = "";
//                                unit.Value = value.Value.ToString();
//                                unit.dh = DateTime.Now;
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (istolog == 1)
//                {
//                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Unexpected error in the data change callback:\n\n" + ex.Message }));
//                }
//            }
//        }

//        private ThreadStart NewThreadLog(Log log)
//        {
//            this.LogWriteRead = new LogWriteRead();
//            this.LogWriteRead.Save(log);
//            return new ThreadStart(() => { });
//        }

//        private ThreadStart NewThreadTagsBulk(List<rInstrumentMeasure> meas)
//        {
//            this.rInstrumentMeasureWriteRead.EditBulk(meas);
//            return new ThreadStart(() => { });
//        }

//        private ThreadStart NewThreadWriteTagBulk(List<rTagWrite> wtag)
//        {
//            this.rTagWriteWriteRead.EditBulk(wtag);
//            return new ThreadStart(() => { });
//        }
//        #endregion

//    }
//}
