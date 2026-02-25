
using System;
using System.ServiceProcess;
using Vale.Tops.Integration.OpcDaDriver.Util;

namespace Vale.Tops.Integration.OpcDaDriver
{
    static class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var svc = new Services.OpcDaPlcService(Settings.ResolveServiceName(args));
            svc.DebugRun();
#else
            ServiceBase.Run(new ServiceBase[] { new Services.OpcDaPlcService(Settings.ResolveServiceName(args)) });
#endif
        }
    }
}
