using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Vale.Tops.Integration.OpcDaDriver.Service
{
    public sealed class ServiceHost : ServiceBase
    {
        private readonly string _svcName;
        private OpcDaPlcRunner _runner;
        private Timer _healthTimer;
        private bool _healthEnabled;
        private int _healthMs;
        private bool _writeEventViewer;

        public ServiceHost(string serviceName)
        {
            _svcName = string.IsNullOrWhiteSpace(serviceName) ? (ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver") : serviceName;
            this.ServiceName = _svcName;
            this.CanStop = true; this.CanPauseAndContinue = false; this.AutoLog = false;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // PATCH: fixar o diretório corrente na pasta do executável (logs/caminhos relativos)
                try
                {
                    System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                    FileLog.Init();
                    FileLog.Info("[BOOT] BaseDirectory=" + AppDomain.CurrentDomain.BaseDirectory);
                    FileLog.Info("[BOOT] ConfigFile=" + AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                }
                catch { }

                // 1) Descricao do App.config
                var desc = ConfigurationManager.AppSettings["Service.Description"];
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    Win32ServiceDescription.TryApplyDescription(this.ServiceName, desc);
                }

                _writeEventViewer = string.Equals(ConfigurationManager.AppSettings["Logs.WriteEventViewer"], "true", StringComparison.OrdinalIgnoreCase);
                EnsureEventSource();

                _healthEnabled = string.Equals(ConfigurationManager.AppSettings["Health.Enabled"], "true", StringComparison.OrdinalIgnoreCase);
                _healthMs = ParseInt("Health.LogIntervalMs", 60000);

                LogInfo($"Service starting... v1.2  Health={(_healthEnabled ? "on" : "off")}({_healthMs}ms)");

                _runner = new OpcDaPlcRunner(LogInfo);
                _runner.OnStatus += s => LogInfo("STATUS: " + s);
                Task.Run(async () =>
                {
                    try { await _runner.StartAsync(); LogInfo("Service started."); }
                    catch (Exception ex) { LogError("StartAsync ERROR: " + ex.Message + "" + ex.StackTrace); throw; }
                });

                if (_healthEnabled)
                {
                    _healthTimer = new Timer(_ =>
                    {
                        try
                        {
                            var st = _runner.SnapshotStatsAndResetDelta();
                            LogInfo($"HEALTH: Connected={st.IsConnected}, Items={st.ItemsCount}, TotalUpdates={st.TotalUpdates}, UpdatesDelta={st.UpdatesDelta}, Queue={st.QueueCount}, LastChangeUTC={(st.LastDataChangeUtc?.ToString("o") ?? "-")}, LastFlushUTC={(st.LastFlushUtc?.ToString("o") ?? "-")}, LastError={(st.LastError ?? "-")}");
                        }
                        catch (Exception ex)
                        {
                            LogError("HealthTimer ERROR: " + ex.Message);
                        }
                    }, null, _healthMs, _healthMs);
                }
            }
            catch (Exception ex)
            {
                LogError("OnStart ERROR: " + ex.Message + "" + ex.StackTrace);
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogInfo("Service stopping...");
                _healthTimer?.Dispose();
                _runner?.StopAsync().GetAwaiter().GetResult();
                LogInfo("Service stopped.");
            }
            catch (Exception ex)
            {
                LogError("OnStop ERROR: " + ex.Message + "" + ex.StackTrace);
            }
        }

        public void DebugStart() => OnStart(null);
        public void DebugStop() => OnStop();

        private void EnsureEventSource()
        {
            const string src = "Vale.OPCDA.Driver";
            try { if (!EventLog.SourceExists(src)) EventLog.CreateEventSource(src, "Application"); } catch { }
        }

        private void LogInfo(string msg)
        {
            FileLog.Info(msg);
            if (_writeEventViewer) try { EventLog.WriteEntry("Vale.OPCDA.Driver", msg, EventLogEntryType.Information); } catch { }
        }
        private void LogError(string msg)
        {
            FileLog.Error(msg);
            if (_writeEventViewer) try { EventLog.WriteEntry("Vale.OPCDA.Driver", msg, EventLogEntryType.Error); } catch { }
        }
        private static int ParseInt(string key, int def) { int v; return int.TryParse(ConfigurationManager.AppSettings[key], out v) ? v : def; }
    }
}
