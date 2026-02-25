
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Vale.Tops.Integration.OpcDaDriver.Service
{
    internal static class FileLog
    {
        private static readonly object _sync = new object();
        private static string _dir;

        public static void Init()
        {
            var path = ConfigurationManager.AppSettings["Logs.Path"];
            if (string.IsNullOrWhiteSpace(path))
            {
                var progData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                path = Path.Combine(progData, "OPCDAService", "logs");
            }
            Directory.CreateDirectory(path);
            _dir = path;
        }

        public static void Write(string level, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_dir)) Init();
                var file = Path.Combine(_dir, "opcda_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";
                lock (_sync)
                {
                    File.AppendAllText(file, line + Environment.NewLine, new UTF8Encoding(false));
                }
            }
            catch { /* não deixa o serviço cair por log */ }
        }

        public static void Info(string msg) => Write("INFO", msg);
        public static void Warn(string msg) => Write("WARN", msg);
        public static void Error(string msg) => Write("ERROR", msg);
    }
}
