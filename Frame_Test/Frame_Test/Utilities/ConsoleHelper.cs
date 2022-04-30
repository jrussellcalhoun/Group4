using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Word_Game.Utilities
{
    public static class ConsoleHelper
    {

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        [Conditional("DEBUG")]
        public static void CreateDebugConsole()
        {
            ConsoleHelper.AllocConsole();
            Trace.WriteLine("Console Start");
            var writer = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(writer);
        }

        [Conditional("DEBUG")]
        public static void ShutdownDebugConsole()
        {
            Console.WriteLine("Console Shutdown");
            System.Threading.Thread.Sleep(1000);
            ConsoleHelper.FreeConsole();
        }

    }
}
