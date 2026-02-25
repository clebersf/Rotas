using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;
using System.Configuration;

namespace Vale.Tops.Integration.Infrastructure.TriggerSp
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        //Declare a variável global do tipo StreamWriter
        static StreamWriter arquivoLog;
        static Timer TimerScan;
        Thread th = new Thread(new ThreadStart(threadMethod));

        protected override void OnStart(string[] args)
        {
            //Instancie a variável criada, que receberá como parâmetro o caminho de meu arquivo de texto, 
            //que será o log destes eventos do meu serviço, e o parâmetro encoding com o valor true.
            EscreveLog("Serviço iniciado !!!");
            th.Start();
        }

        static void threadMethod()
        {
            string Rate = ConfigurationManager.AppSettings["Rate"];
            TimerScan = new Timer(new TimerCallback(TimerScan_Tick), null, Convert.ToInt16(Rate), Convert.ToInt16(Rate));            
        }

        protected override void OnStop()
        {
            EscreveLog("Serviço parado !!!");
        }

        static void TimerScan_Tick(object sender)
        {
            try
            {
                OptionExecSp obj = new OptionExecSp();
                Thread tl = new Thread(obj.ExecSpNPar());
            }
            catch (Exception ex)
            {
                EscreveLog(ex.Message);
            }
        }

        static protected void EscreveLog(string MsgLog)
        {
            //Instancie a variável criada, que receberá como parâmetro o caminho de meu arquivo de texto, 
            //que será o log destes eventos do meu serviço, e o parâmetro encoding com o valor true.
            arquivoLog = new StreamWriter(@ConfigurationManager.AppSettings["CaminhoLog"], true);

            //Escrevo no arquivo texto no momento exato que o arquivo for iniciado
            arquivoLog.WriteLine(MsgLog + " " + DateTime.Now);

            //Limpo o buffer com o método Flush
            arquivoLog.Flush();
            arquivoLog.Close();
        }

    }
}
