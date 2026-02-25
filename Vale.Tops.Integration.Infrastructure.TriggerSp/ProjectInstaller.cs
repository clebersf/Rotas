using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Configuration;
using System.ServiceProcess;
using System.Reflection;


namespace Vale.Tops.Integration.Infrastructure.TriggerSp
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {

        //InitializeComponent();internal static string ServiceNameDefault = "My Service";

        internal static string ServiceName = GetConfigurationValue("ServiceName");
        internal static string DisplayName = GetConfigurationValue("DisplayName");
        internal static string Description = GetConfigurationValue("Description");

        /// <summary>
        /// Public Constructor for WindowsServiceInstaller.
        /// - Put all of your Initialization code here.
        /// </summary>
        public ProjectInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            //serviceProcessInstaller.Username = null;
            //serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = DisplayName;
            serviceInstaller.ServiceName = ServiceName;
            serviceInstaller.Description = Description;
            serviceInstaller.StartType = ServiceStartMode.Manual;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = ServiceName;

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }

        private static string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(Service));

            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);

            return config.AppSettings.Settings[key].Value;
        }
    }
}
