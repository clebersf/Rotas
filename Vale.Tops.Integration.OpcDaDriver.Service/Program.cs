
using System;
using System.Configuration;
using System.ServiceProcess;

namespace Vale.Tops.Integration.OpcDaDriver.Service
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string svcName = ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver";

            if (Environment.UserInteractive || (args != null && Array.Exists(args, a => a.Equals("/console", StringComparison.OrdinalIgnoreCase))))
            {
                var svc = new ServiceHost(svcName);
                svc.DebugStart();
                Console.WriteLine($"[{svcName}] rodando em modo CONSOLE. Pressione ENTER para encerrar...");
                Console.ReadLine();
                svc.DebugStop();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] { new ServiceHost(svcName) });
            }
        }
    }
}
