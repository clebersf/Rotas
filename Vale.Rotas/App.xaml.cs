using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Vale.Rotas
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // 1. CONTROLE DE INSTÂNCIA ÚNICA (MUTEX)
        // 🚨 IMPORTANTE: SUBSTITUA ESTE GUID por um valor único para a sua aplicação!
        private const string AppGuid = "216A46C4-3F2A-4D4B-A6C1-6A795C2D0E6E";
        private static Mutex _mutex;

        // 2. IMPORTAÇÕES WIN32 PARA CONTROLE DE JANELA E FOCO

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

        // Novas importações para ativação robusta de janela (resolvendo problemas intermitentes de foco)
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();


        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        private struct Windowplacement
        {
            public int length;
            public int flags;
            public int showCmd;
        }

        // 3. ATIVAÇÃO ROBUSTA DA JANELA (Função para forçar o foreground)
        private void ForceForegroundWindow(IntPtr hWnd)
        {
            // A. Restaurar (se minimizada) e trazer a janela para o topo
            ShowWindowAsync(hWnd, (int)Actions.Restore);
            BringWindowToTop(hWnd); // Tentativa rápida de trazer a janela para frente

            // B. Anexar Threads para forçar o foco (o truque principal)
            IntPtr foregroundHandle = GetForegroundWindow();
            if (hWnd == foregroundHandle)
            {
                // Já está em primeiro plano, não faça nada.
                return;
            }

            uint currentThreadId = GetCurrentThreadId();
            uint foregroundThreadId = GetWindowThreadProcessId(foregroundHandle, out _);

            if (currentThreadId != foregroundThreadId)
            {
                // 1. Anexa temporariamente o thread da janela de fundo ao thread da sua aplicação.
                AttachThreadInput(currentThreadId, foregroundThreadId, true);

                // 2. Tenta definir o foco (o Windows agora permite)
                SetForegroundWindow(hWnd);

                // 3. Desanexa o thread.
                AttachThreadInput(currentThreadId, foregroundThreadId, false);
            }
            else
            {
                // Se a chamada for no mesmo thread (situação rara), usa o SetForegroundWindow normal
                SetForegroundWindow(hWnd);
            }
        }

        // 4. RESTAURAÇÃO DA INSTÂNCIA EXISTENTE (Chamado pela 2ª instância)
        private void RestoreExistingInstance()
        {
            // O nome do processo é o nome do executável (geralmente o nome do projeto)
            string procName = "Vale.Rotas";

            // Busca o processo existente (o que não é o atual)
            Process existingProcess = Process.GetProcessesByName(procName)
                .FirstOrDefault(p => p.Id != Process.GetCurrentProcess().Id);

            if (existingProcess != null)
            {
                IntPtr handle = existingProcess.MainWindowHandle;

                if (handle != IntPtr.Zero)
                {
                    // Usa a função robusta para garantir que a janela fique em primeiro plano
                    ForceForegroundWindow(handle);
                }
            }
        }

        private void Minimize()
        {
            IntPtr wdwIntPtr = FindWindow(null, "Sistema Rotas - VALE");

            Windowplacement placement = new Windowplacement();
            GetWindowPlacement(wdwIntPtr, ref placement);

            // Se a janela não estiver minimizada
            if (placement.showCmd != 2) // 2 é ShowMinimized
            {
                Action("Sistema Rotas - VALE", Actions.Minimize);
            }
        }

        void App_Deactivated(object sender, EventArgs e)
        {
            // Acessa a janela principal e verifica se janelas secundárias estão abertas
            var mw = Application.Current.MainWindow as Vale.Rotas.MainWindow;

            if (mw != null)
            {
                // Se nenhuma janela secundária de controle (wcomm) estiver aberta, minimiza
                if (!mw.wcomm.isOpen_Add && !mw.wcomm.isOpen_Consistency)
                    Minimize();
            }
        }

        // 5. PONTO DE ENTRADA: CONTROLE DE INSTÂNCIA
        void App_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            // Tenta obter a posse do Mutex
            _mutex = new Mutex(true, "Global\\" + AppGuid, out createdNew);

            if (!createdNew)
            {
                // APLICAÇÃO JÁ ESTÁ RODANDO!
                RestoreExistingInstance();

                // Encerra a nova instância imediatamente
                Application.Current.Shutdown();
            }
            // Se createdNew for true, a aplicação continua a inicialização normal.

            // Note: O código original tinha Restore() e BringWindowToFront() aqui.
            // Eles foram removidos pois a lógica de ativação agora é feita dentro do controle do Mutex.
        }

        public enum Actions { Normal = 1, Minimize = 2, Maximize = 3, Show = 5, Restore = 9 };

        //Importa o user32.dll para poder usar as APIs nativas
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        //Busca um aplicativo pelo nome (Atenção: busca pelo nome do executável)
        public static IntPtr FindWindow(string title)
        {
            // Este método FindWindow é confuso, pois recebe "title" mas usa Process.GetProcessesByName.
            // O nome do processo é "Vale.Rotas", então o 'title' aqui é o nome do PROCESSO.
            Process[] pros = Process.GetProcessesByName(title);

            if (pros.Length == 0)
                return IntPtr.Zero;

            return pros[0].MainWindowHandle;
        }

        //Dispara a ação desejada
        public static void Action(string name, Actions act)
        {
            // Este método é usado por Minimize(), que passa o TÍTULO da janela ("Sistema Rotas - VALE")
            // mas o FindWindow abaixo busca por nome do PROCESSO. Isso pode ser um problema.
            // O FindWindow original (DllImport) precisa ser usado aqui.

            // Usando a DllImport original que busca por título:
            IntPtr hWnd = FindWindow(null, name);

            if (!hWnd.Equals(IntPtr.Zero))
                ShowWindowAsync(hWnd, (int)act);
        }
    }
}