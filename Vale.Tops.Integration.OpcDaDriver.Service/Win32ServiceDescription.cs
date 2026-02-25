using System;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Vale.Tops.Integration.OpcDaDriver.Service
{
    internal static class Win32ServiceDescription
    {
        private const int SC_MANAGER_ALL_ACCESS = 0xF003F;
        private const int SERVICE_CHANGE_CONFIG = 0x0002;
        private const int SERVICE_CONFIG_DESCRIPTION = 1;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SERVICE_DESCRIPTION
        {
            public string lpDescription;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName, int dwDesiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, int dwDesiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ChangeServiceConfig2(IntPtr hService, int dwInfoLevel, ref SERVICE_DESCRIPTION lpInfo);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);

        public static void TryApplyDescription(string serviceName, string description)
        {
            if (string.IsNullOrWhiteSpace(serviceName) || string.IsNullOrWhiteSpace(description))
                return;

            IntPtr scm = IntPtr.Zero, svc = IntPtr.Zero;
            try
            {
                scm = OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
                if (scm == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error(), "OpenSCManager failed.");

                svc = OpenService(scm, serviceName, SERVICE_CHANGE_CONFIG);
                if (svc == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error(), "OpenService failed.");

                var sd = new SERVICE_DESCRIPTION { lpDescription = description };
                if (!ChangeServiceConfig2(svc, SERVICE_CONFIG_DESCRIPTION, ref sd))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "ChangeServiceConfig2 failed.");
            }
            catch (Exception)
            {
                // não derruba o serviço por causa da descrição; silencie ou logue se quiser
            }
            finally
            {
                if (svc != IntPtr.Zero) CloseServiceHandle(svc);
                if (scm != IntPtr.Zero) CloseServiceHandle(scm);
            }
        }
    }
}
