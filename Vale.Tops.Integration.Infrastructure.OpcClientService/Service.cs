using Siemens.Opc.Da;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Siemens.Opc;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Configuration;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.CodeDom;
using System.Security.Policy;

namespace Vale.Tops.Integration.Infrastructure.OpcClientService
{

    public partial class Opc_Plc
    {
        public Subscription m_Subscription { get; set; }
        public long PlcId { get; set; }
        public string Name { get; set; }

        public DateTime dh { get; set; }

        public Opc_Plc() { }
    }

    public partial class Items
    {
        public long Id { get; set; }
        public long TagId { get; set; }
        public string PlcName { get; set; }
        public string TagName { get; set; }
        public Items() { }
    }


    public partial class Service : ServiceBase
    {

        #region Private Members
        private Server m_Server = new Server();
        private Subscription m_Subscription = null;
        private List<Opc_Plc> plcs = new List<Opc_Plc>();
        private List<Tag> itens = new List<Tag>();

        private ILogWriteRead LogWriteRead;
        private IrInstrumentMeasureWriteRead rInstrumentMeasureWriteRead;
        private IrTagWriteWriteRead rTagWriteWriteRead;
        private ITagWriteRead TagWriteRead;
        private IPlcWriteRead PlcWriteRead;
        private ILocationWriteRead LocationWriteRead;
        private IrTagGroupWriteRead rTagGroupWriteRead;
        private string txtServerUrlText = "";
        private System.Timers.Timer _Timer_Bd;
        private System.Timers.Timer _Timer_Wr;

        private int ParentId = 0;
        private int ApplicationId = 0;
        private int LocationId = 0;
        private int istolog = 0;
        private int iswrite = 0;
        //private int isautoreset = 0;
        private int cycleautoreset = 0;
        //private int contcycleautoreset = 0;
        private int rate = 0;
        private string initopic = "";
        private string fimtopic = "";
        private string servicename = "";

        private string ip = "127.0.0.1";
        private string pingStatus = "";

        private List<rInstrumentMeasure> meas = new List<rInstrumentMeasure>();
        private List<rInstrumentMeasure> genmeas = new List<rInstrumentMeasure>();
        private List<Items> items = new List<Items>();
        #endregion

        #region Construction

        public Service()
        {
            InitializeComponent();
        }

        #endregion

        #region Service methods
        //protected override void OnStart(string[] args)
        // protected override void OnStop()

        private ThreadStart InitializeDriver()
        {
            try
            {
                // Configuração leitura e escrita banco de dados
                this.LocationWriteRead = new LocationWriteRead();
                this.rTagGroupWriteRead = new rTagGroupWriteRead();
                this.TagWriteRead = new TagWriteRead();
                this.PlcWriteRead = new PlcWriteRead();
                this.rInstrumentMeasureWriteRead = new rInstrumentMeasureWriteRead();
                this.rTagWriteWriteRead = new rTagWriteWriteRead();
                this.LogWriteRead = new LogWriteRead();
                // set the sever we want to connet to
                ParentId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ParentId"]);
                istolog = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IsToLog"]);
                iswrite = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IsWrite"]);
                initopic = System.Configuration.ConfigurationManager.AppSettings["IniTopic"];
                fimtopic = System.Configuration.ConfigurationManager.AppSettings["FimTopic"];
                var ServerUrl = this.rTagGroupWriteRead.All().Where(p => p.Id.Equals(ParentId));
                txtServerUrlText = "opcda://" + ServerUrl.First().AddrOpcServer + "/" + ServerUrl.First().OpcServer;
                ApplicationId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ApplicationId"]);
                LocationId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["LocationId"]);
                servicename = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
                ip = System.Configuration.ConfigurationManager.AppSettings["IP"];
                rate = Convert.ToInt16(ServerUrl.First().Rate);

            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "InitializeDriver: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
            return new ThreadStart(() => { });
        }

        private ThreadStart ReadBd()
        {
            try
            {
                List<rInstrumentMeasure> measures = new List<rInstrumentMeasure>();
                genmeas = (from rmeas in this.rInstrumentMeasureWriteRead.All()
                           join tg in this.rTagGroupWriteRead.All().Where(p => p.ParentId == ParentId).ToList() on rmeas.TagId equals tg.TagId
                           select rmeas).ToList();
                meas = (from data in (from rmeas in this.rInstrumentMeasureWriteRead.All()
                                      join tgg in this.rTagGroupWriteRead.All().Where(p => p.ParentId == ParentId).ToList() on rmeas.TagId equals tgg.TagId
                                      join tg in this.TagWriteRead.All() on tgg.TagId equals tg.Id
                                      select new
                                      {
                                          Name = tg.Name,
                                          PlcId = tg.PlcId,
                                          Id = rmeas.Id,
                                          dh = rmeas.dh,
                                          LastDh = rmeas.LastDh,
                                          Location = rmeas.Location,
                                          Tag = rmeas.Tag,
                                          TagId = rmeas.TagId,
                                          Value = rmeas.Value,
                                          Write = rmeas.Write
                                      })
                        group data by new { Name = data.Name, PlcId = data.PlcId } into g
                        select new rInstrumentMeasure
                        {
                            Id = g.FirstOrDefault().Id,
                            dh = g.FirstOrDefault().dh,
                            ErrorCode = "",
                            ErrorString = "",
                            isUpdating = false,
                            LastDh = g.Max(p => p.LastDh),
                            LastValue = "",
                            Location = g.FirstOrDefault().Location,
                            Tag = g.FirstOrDefault().Tag,
                            TagId = g.FirstOrDefault().TagId,
                            Value = g.FirstOrDefault().Value,
                            Write = g.FirstOrDefault().Write
                        }).ToList();

                items = (from m in meas
                         join tg in this.TagWriteRead.All() on m.TagId equals tg.Id
                         join plc in this.LocationWriteRead.All() on tg.PlcId equals plc.Id
                         select new Items { Id = m.Id, PlcName = plc.Name, TagName = initopic + plc.Name + fimtopic + tg.Name, TagId = tg.Id }).ToList();
            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "ReadBd: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
            return new ThreadStart(() => { });
        }
        //protected override void OnStart(string[] args)
        // protected override void OnStop()
        protected override void OnStart(string[] args) 
        {
            try
            {
                Thread tl1 = new Thread(InitializeDriver());
                Thread tl2 = new Thread(ReadBd());
                OnConnect();

                this._Timer_Bd = new System.Timers.Timer();
                this._Timer_Bd.Elapsed += new
                System.Timers.ElapsedEventHandler(timer_Tick_Bd);
                this._Timer_Bd.Enabled = false;
                this._Timer_Bd.Interval = rate;
                this._Timer_Bd.Enabled = true;

                if (iswrite == 1)
                {
                    this._Timer_Wr = new System.Timers.Timer();
                    this._Timer_Wr.Elapsed += new
                    System.Timers.ElapsedEventHandler(timer_Tick_Wr);
                    this._Timer_Wr.Enabled = false;
                    this._Timer_Wr.Interval = rate;
                    this._Timer_Wr.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "OnStart: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
            finally
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Service Opc Client Started" + " Service Name: " + servicename }));
                }
            }
        }

        protected override void OnStop()
        {
            OnDisconnect();
            ShutDownRequest("");
            this._Timer_Bd.Stop();
            if (istolog == 1)
            {
                Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = 1, Message = "Service Opc Client ended" + " Service Name: " + servicename }));
            }
        }

        public async Task PingClp()
        {
            try
            {
                Ping pinger = new Ping();
                PingReply response = await pinger.SendPingAsync(ip);
                pingStatus = response.Status.ToString();
            }
            catch (Exception ex) { throw; }
        }

        private void timer_Tick_Wr(object sender,
      System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Task task = PingClp();
                if (m_Server != null)
                {
                    if (iswrite == 1 & m_Server.isConnected() & pingStatus == "Success")
                        WriteTags();
                }
            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "timer_Tick_Wr: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
        }
        private void timer_Tick_Bd(object sender,
  System.Timers.ElapsedEventArgs e)
        {

            List<rInstrumentMeasure> tpmeas = new List<rInstrumentMeasure>();
            try
            {
                Task task = PingClp();
                if (m_Server != null)
                {
                    if (m_Server.isConnected() & pingStatus == "Success")
                    {
                        StringCollection itemIds = new StringCollection();
                        object[] values;
                        int[] pErrors;
                        foreach (var it in items)
                        {
                            itemIds.Add(it.TagName);
                        }

                        m_Server.Read(itemIds, out values, out pErrors);

                        int i = 0;
                        foreach (var it in items)
                        {
                            var un = meas.Where(p => p.TagId == it.TagId).FirstOrDefault();
                            if (pErrors[i] == 0)
                            {
                                if (values[i] != null)
                                {
                                    un.dh = DateTime.Now;
                                    un.Value = values[i].ToString();
                                    tpmeas.Add(un);
                                }
                            }
                            i++;
                        }
                        var upd = (from gm in genmeas
                                   join m in tpmeas on gm.TagId equals m.TagId
                                   select new rInstrumentMeasure
                                   {
                                       Id = gm.Id,
                                       dh = DateTime.Now,
                                       ErrorCode = "",
                                       ErrorString = "",
                                       isUpdating = false,
                                       LastDh = gm.dh,
                                       LastValue = "",
                                       Location = gm.Location,
                                       Tag = gm.Tag,
                                       TagId = gm.TagId,
                                       Value = m.Value,
                                       Write = gm.Write
                                   }).ToList();
                        this.rInstrumentMeasureWriteRead.EditBulk(upd);
                    }
                }
            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "timer_Tick_Bd: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
        }
        #endregion

        #region Connect and Disconnect Server
        /// <summary>
        /// Handles connect procedure
        /// </summary>
        private void OnConnect()
        {
            if (m_Server == null)
            {
                // Create a server object
                m_Server = new Server();
            }

            try
            {
                // connect to the server
                if (!m_Server.isConnected())
                    m_Server.Connect(txtServerUrlText, rate, ParentId);
            }
            catch (Exception ex)
            {
                // Cleanup
                m_Server = null;
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "OnConnect: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }

        }

        /// <summary>
        /// Handles disconnect procedure
        /// </summary>
        private void OnDisconnect()
        {
            if (m_Server == null)
            {
                return;
            }

            try
            {
                // Disconnect
                m_Server.Disconnect();
                m_Server.Dispose();
                m_Server = null;
            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "OnDisconnect: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
        }

        #endregion

        #region Tags iteraction

        private void WriteTags()
        {
            try
            {
                this.rTagGroupWriteRead = new rTagGroupWriteRead();
                this.rTagWriteWriteRead = new rTagWriteWriteRead();
                var wtags = this.rTagGroupWriteRead.All().Where(p => p.ParentId == ParentId).ToList();
                var wgates = this.rTagWriteWriteRead.All().Where(p => p.Write == true);
                var wtaggate = (from wgate in wgates
                                join wtag in wtags on wgate.Id equals wtag.TagId
                                select new { Tag = wtag.Tag, Value = wgate.Value }).ToList();
                var update = (from wgate in wgates
                              join wtag in wtags on wgate.Id equals wtag.TagId
                              select new rTagWrite { Id = wgate.Id, Tag = wgate.Tag, Value = wgate.Value, Write = false }).ToList();
                Thread tl = new Thread(NewThreadWriteTagBulk(update));
                foreach (var unit in wtaggate)
                {
                    try
                    {
                        m_Server.Write(initopic + unit.Tag.Plc.Location.Name + fimtopic + unit.Tag.Name, unit.Value);
                    }
                    catch (Exception ex)
                    {
                        if (istolog == 1)
                        {
                            Thread tlog = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "OnDisconnect: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (istolog == 1)
                {
                    Thread tl = new Thread(NewThreadLog(new Log { Id = Guid.NewGuid(), ApplicationId = ApplicationId, dh = DateTime.Now, LocationId = LocationId, Message = "OnDisconnect: " + ex.Message + " Details: " + ex.InnerException + " Where: " + ex.StackTrace + " Service Name: " + servicename }));
                }
            }
        }

        #endregion


        #region Event Handlers
        /// <summary>
        /// Show shutdown message
        /// When receiving a shutdown message just disconnect.
        /// </summary>
        public void ShutDownRequest(string reason)
        {
            OnDisconnect();
        }

        /// <summary>
        /// callback to receive datachanges
        /// </summary>
        /// <param name="clientHandle"></param>
        /// <param name="value"></param>

        private ThreadStart NewThreadLog(Log log)
        {
            this.LogWriteRead = new LogWriteRead();
            this.LogWriteRead.Save(log);
            return new ThreadStart(() => { });
        }

        private ThreadStart NewThreadWriteTagBulk(List<rTagWrite> wtag)
        {
            this.rTagWriteWriteRead.EditBulk(wtag);
            return new ThreadStart(() => { });
        }
        #endregion

    }
}
