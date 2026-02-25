
using System;
using System.Configuration;

namespace Vale.Tops.Integration.OpcDaDriver.Util
{
    internal static class Settings
    {
        public static string ResolveServiceName(string[] args)
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var a = args[i] ?? string.Empty;
                    if (a.StartsWith("--name", StringComparison.OrdinalIgnoreCase) || a.StartsWith("/name", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = a.Split(new[] { '=', ':' }, 2);
                        if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[1]))
                            return parts[1].Trim();
                        if (i + 1 < args.Length) return args[i + 1];
                    }
                }
            }
            var env = Environment.GetEnvironmentVariable("VALE_OPC_SERVICE_NAME");
            if (!string.IsNullOrWhiteSpace(env)) return env.Trim();
            return ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver";
        }
    }
}
