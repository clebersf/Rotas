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

//    public partial class Opc_Plc
//    {
//        public Subscription m_Subscription { get; set; }
//        public long PlcId { get; set; }
//        public string Name { get; set; }

//        public DateTime dh { get; set; }

//        public Opc_Plc() { }
//    }

//    public partial class Service : ServiceBase
//    {


//        #region Private Members
//        private Server m_Server = new Server();
//        private Subscription m_Subscription = null;
//        private List<Opc_Plc> plcs = new List<Opc_Plc>();
//        private List<Tag> itens = new List<Tag>();

//        private ILogWriteRead LogWriteRead;
//        private IrInstrumentMeasureWriteRead rInstrumentMeasureWriteRead;
//        private IrTagWriteWriteRead rTagWriteWriteRead;
//        private ITagWriteRead TagWriteRead;
//        private IPlcWriteRead PlcWriteRead;
//        private IrTagGroupWriteRead rTagGroupWriteRead;
//        private string txtServerUrlText = "";
//        private System.Timers.Timer _Timer_Write;
//        private System.Timers.Timer _Timer_Bd;
//        private System.Timers.Timer _Timer_Wd;

//        private int ParentId = 0;
//        private int npool = 0;
//        private int ApplicationId = 0;
//        private int istolog = 0;
//        private int iswrite = 0;
//        private int isupdating = 0;
//        private int whatchdog = 0;
//        private int rate = 0;
//        private string initopic = "";
//        private string fimtopic = "";
//        private DateTime lastdh = DateTime.Now;


//        private List<rInstrumentMeasure> meas = new List<rInstrumentMeasure>();
//        private List<rInstrumentMeasure> tempmeas = new List<rInstrumentMeasure>();
//        private List<rTagGroup> tags = new List<rTagGroup>();
//        #endregion

//        #region Construction

//        public Service()
//        {
//            InitializeComponent();

//            try
//            {
//                // Configuração leitura e escrita banco de dados
//                this.rTagGroupWriteRead = new rTagGroupWriteRead();
//                this.TagWriteRead = new TagWriteRead();
//                this.PlcWriteRead = new PlcWriteRead();
//                //this.vw_Opc_TagGroup_ReadOnly = new vw_Opc_TagGroup_ReadOnly();
//                this.rInstrumentMeasureWriteRead = new rInstrumentMeasureWriteRead();
//                this.rTagWriteWriteRead = new rTagWriteWriteRead();
//                this.LogWriteRead = new LogWriteRead();
//                // set the sever we want to connet to
//                ParentId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ParentId"]);
//                istolog = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IsToLog"]);
//                iswrite = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IsWrite"]);
//                initopic = System.Configuration.ConfigurationManager.AppSettings["IniTopic"];
//                fimtopic = System.Configuration.ConfigurationManager.AppSettings["FimTopic"];
//                var ServerUrl = this.rTagGroupWriteRead.All().Where(p => p.Id.Equals(ParentId));
//                txtServerUrlText = "opcda://" + ServerUrl.First().AddrOpcServer + "/" + ServerUrl.First().OpcServer;
//                ApplicationId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ApplicationId"]);
//                whatchdog = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WhatchDog"]);
//                rate = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]);

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

//                //this._Timer_Wd = new System.Timers.Timer();
//                //this._Timer_Wd.Elapsed += new
//                //System.Timers.ElapsedEventHandler(timer_Tick_Wd);
//                //this._Timer_Wd.Enabled = false;
//                //this._Timer_Wd.Interval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WhatchDog"]);

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Falha ao iniciar o serviço. - " + ex.InnerException);
//            }

//        }

//        private void ReadBd()
//        {
//            var plcsax = plcs;
//            tags = this.rTagGroupWriteRead.All().Where(p => p.ParentId == ParentId).ToList();
//            // Consulta tabela de tags/valores
//            meas = (from rmeas in this.rInstrumentMeasureWriteRead.All()
//                    join tg in tags on rmeas.TagId equals tg.TagId
//                    select rmeas).ToList();

//            plcs = (from data in (from tggroup in this.rTagGroupWriteRead.All().Where(p => p.ParentId == ParentId)
//                                  join tg in this.TagWriteRead.All() on tggroup.TagId equals tg.Id
//                                  join plc in this.PlcWriteRead.All() on tg.PlcId equals plc.Id
//                                  join values in meas on tg.Id equals values.TagId
//                                  select new { plc.Id, plc.Location.Name, values.dh })
//                    group data by data.Id into g
//                    select new Opc_Plc { m_Subscription = null, dh = g.Max(p => p.dh), Name = g.First().Name, PlcId = g.First().Id }).ToList();

//            foreach (var pl in plcsax)
//            {
//                plcs.Where(p => p.PlcId == pl.PlcId).First().m_Subscription = pl.m_Subscription;
//            }
//        }

//        #endregion

//        #region Service methods
//        //protected override void OnStart(string[] args)
//        // protected override void OnStop()
//        protected override void OnStart(string[] args)
//        {
//            try
//            {
//                OnConnect();
//                startMonitorItems();
//                if (iswrite == 1)
//                    this._Timer_Write.Enabled = true;

//                this._Timer_Bd.Enabled = true;
//                //this._Timer_Wd.Enabled = true;
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
//            var tpmeas = tempmeas.ToList();
//            try
//            {
//                foreach (var tp in tpmeas)
//                {
//                    var unit = meas.Where(p => p.TagId == tp.TagId).OrderByDescending(p => p.dh).FirstOrDefault();
//                    if (unit != null)
//                    {
//                        if (tpmeas.Where(p => p.TagId == unit.TagId).OrderByDescending(p => p.dh).FirstOrDefault() != null)
//                        {
//                            unit.Value = tpmeas.Where(p => p.TagId == unit.TagId).OrderByDescending(p => p.dh).FirstOrDefault().Value;
//                            unit.dh = tpmeas.Where(p => p.TagId == unit.TagId).OrderByDescending(p => p.dh).FirstOrDefault().dh;
//                            unit.ErrorCode = tpmeas.Where(p => p.TagId == unit.TagId).OrderByDescending(p => p.dh).FirstOrDefault().ErrorCode;
//                            unit.ErrorString = tpmeas.Where(p => p.TagId == unit.TagId).OrderByDescending(p => p.dh).FirstOrDefault().ErrorString;
//                        }
//                    }
//                }
//                tempmeas.Clear();
//                //Thread tl = new Thread(NewThreadTagsBulk(meas));
//                this.rInstrumentMeasureWriteRead.EditBulk(meas);
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

//        private void timer_Tick_Wd(object sender,
//          System.Timers.ElapsedEventArgs e)
//        {
//            try
//            {
//                CheckMonitorItems();
//                this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Atualizado o PLC" });
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
//                m_Server.Connect(txtServerUrlText, rate);
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
//                this.rTagGroupWriteRead = new rTagGroupWriteRead();
//                this.rTagWriteWriteRead = new rTagWriteWriteRead();
//                var wtags = this.rTagGroupWriteRead.All().Where(p => p.ParentId == ParentId).ToList();
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
//                        m_Server.Write(initopic + unit.Tag.Plc.Location.Name + fimtopic + unit.Tag.Name, unit.Value);
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
//        void startMonitorItems()
//        {
//            ReadBd();
//            int i = 0;
//            foreach (Opc_Plc s in plcs)
//            {
//                try
//                {
//                    // Create subscription
//                    var sub = m_Server.CreateSubscription(s.Name, OnDataChange);
//                    s.m_Subscription = sub;
//                    foreach (var tag in tags.Where(p => p.Tag.PlcId == s.PlcId).ToList())
//                    {
//                        try
//                        {
//                            sub.AddItem(initopic + s.Name + fimtopic + tag.Tag.Name, ((int)tag.TagId));
//                        }
//                        catch (Exception exception)
//                        {
//                            if (istolog == 1)
//                                this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Start Monitor - TAG - " + exception.Message });
//                        }
//                    }
//                    plcs[i].m_Subscription = sub;
//                    i++;
//                }
//                catch (Exception exception)
//                {
//                    if (istolog == 1)
//                        this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Start Monitor - PLC - " + exception.Message });
//                }
//            }
//        }


//        void stopMonitorItems()
//        {

//            foreach (Opc_Plc s in plcs)
//            {
//                try
//                {
//                    foreach (var tag in tags.Where(p => p.Tag.PlcId == s.PlcId).ToList())
//                    {
//                        s.m_Subscription.RemoveItem(initopic + s.Name + fimtopic + tag.Tag.Name);
//                    }
//                    m_Server.DeleteSubscription(s.m_Subscription);
//                }
//                catch (Exception ex)
//                {
//                    if (istolog == 1)
//                        this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Stop Monitor - " + ex.Message });
//                }
//            }
//        }

//        void CheckMonitorItems()
//        {
//            ReadBd();
//            int i = 0;
//            foreach (Opc_Plc s in plcs)
//            {
//                try
//                {
//                    var f = s.dh.AddMilliseconds(whatchdog);
//                    if (f <= DateTime.Now)
//                    {
//                        foreach (var tag in tags.Where(p => p.Tag.PlcId == s.PlcId).ToList())
//                        {
//                            var value_saved = meas.Where(p => p.TagId == tag.TagId).First().Value;
//                            var value_current = m_Server.Read(initopic + s.Name + fimtopic + tag.Tag.Name);
//                            if (value_saved == value_current.ToString())
//                            {
//                                Reset();
//                            }
//                        }
//                    }
//                    i++;
//                }
//                catch (Exception ex)
//                {
//                    if (istolog == 1)
//                        this.LogWriteRead.Save(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Check Monitor - " + ex.Message });
//                }
//            }
//        }

//        void Reset()
//        {
//            this._Timer_Write.Enabled = false;
//            this._Timer_Bd.Enabled = false;
//            this._Timer_Write.Dispose();
//            this._Timer_Bd.Dispose();
//            //Stop Service
//            stopMonitorItems();
//            OnDisconnect();
//            ShutDownRequest("");


//            //Start Service
//            OnConnect();
//            startMonitorItems();

//            this._Timer_Write = new System.Timers.Timer();
//            this._Timer_Write.Elapsed += new
//            System.Timers.ElapsedEventHandler(timer_Tick_Write);
//            this._Timer_Write.Enabled = false;
//            this._Timer_Write.Interval = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]);

//            this._Timer_Bd = new System.Timers.Timer();
//            this._Timer_Bd.Elapsed += new
//            System.Timers.ElapsedEventHandler(timer_Tick_Bd);
//            this._Timer_Bd.Enabled = false;
//            this._Timer_Bd.Interval = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]);

//            this._Timer_Write.Enabled = true;
//            this._Timer_Bd.Enabled = true;
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
//            rInstrumentMeasure unit = new rInstrumentMeasure();
//            try
//            {
//                foreach (DataValue value in DataValues)
//                {
//                    TimeSpan ts = DateTime.Now.Subtract(meas.Where(p => p.TagId == value.ClientHandle).First().dh);
//                    int diff = ts.Seconds * 1000 + ts.Milliseconds;
//                    if (diff > Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Rate"]))
//                    {
//                        if (value.Error != 0)
//                        {
//                            var foundtag = meas.Where(item => item.TagId == value.ClientHandle).FirstOrDefault();
//                            unit.Id = foundtag.Id;
//                            unit.TagId = foundtag.TagId;
//                            unit.LastDh = foundtag.LastDh;
//                            unit.ErrorCode = value.Error.ToString();
//                            unit.ErrorString = value.Quality.ToString();
//                            unit.dh = DateTime.Now;
//                        }
//                        else
//                        {
//                            var foundtag = meas.Where(item => item.TagId == value.ClientHandle).FirstOrDefault();
//                            unit.Id = foundtag.Id;
//                            unit.TagId = foundtag.TagId;
//                            unit.LastDh = foundtag.LastDh;
//                            unit.ErrorCode = "";
//                            unit.ErrorString = "";
//                            unit.dh = DateTime.Now;
//                            unit.Value = value.Value != null ? value.Value.ToString() : "";
//                        }
//                    }
//                    tempmeas.Add(unit);
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
//            var copymeas = new List<rInstrumentMeasure>();
//            copymeas.AddRange(meas);
//            this.rInstrumentMeasureWriteRead.EditBulk(copymeas);
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
