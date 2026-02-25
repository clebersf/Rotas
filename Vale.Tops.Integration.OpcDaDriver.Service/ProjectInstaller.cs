using System;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Vale.Tops.Integration.OpcDaDriver.Service
{
    [RunInstaller(true)]
    public sealed class ProjectInstaller : Installer
    {
        private readonly ServiceInstaller _svc;
        private readonly ServiceProcessInstaller _proc;

        public ProjectInstaller()
        {
            // 1) Lê chaves do App.config
            var svcName = ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver";
            var svcDesc = ConfigurationManager.AppSettings["Service.Description"] ?? string.Empty;

            // (opcional) conta e modo de start via App.config
            var startModeText = ConfigurationManager.AppSettings["Service.StartMode"] ?? "Automatic";
            if (!Enum.TryParse(startModeText, true, out ServiceStartMode startMode))
                startMode = ServiceStartMode.Automatic;

            var accountText = ConfigurationManager.AppSettings["Service.Account"] ?? "LocalSystem";
            if (!Enum.TryParse(accountText, true, out ServiceAccount account))
                account = ServiceAccount.LocalSystem;

            var username = ConfigurationManager.AppSettings["Service.Username"];
            var password = ConfigurationManager.AppSettings["Service.Password"];

            // 2) Configura conta do serviço
            _proc = new ServiceProcessInstaller
            {
                Account = account
            };
            if (account == ServiceAccount.User)
            {
                _proc.Username = username;
                _proc.Password = password;
            }

            // 3) Configura instalador do serviço
            _svc = new ServiceInstaller
            {
                ServiceName = svcName,
                DisplayName = svcName,
                StartType = startMode,
                Description = svcDesc
            };

            // 4) Registra
            Installers.AddRange(new Installer[] { _proc, _svc });

            // 5) Belt-and-suspenders: garante descrição após instalar
            this.AfterInstall += (s, e) =>
            {
                try
                {
                    Win32ServiceDescription.TryApplyDescription(_svc.ServiceName, svcDesc);
                }
                catch
                {
                    // Não falha instalação por causa da descrição
                }
            };
        }
    }
}