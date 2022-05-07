using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WordGame.Utilities
{
    /// <summary>
    /// This is a static helper class that we used for logging output while designing our application.
    /// The code here is marked with the [Conditional("DEBUG")] attribute which instructs the compiler
    /// that these methods are only to be executed when we are in a build where the DEBUG flag has been
    /// set. So in Release versions this code is essentially skipped and does not impact performance.
    /// The only thing this does is make a DLL call to the external windows system library kernel32.dll
    /// in order to create a console window attached to our program. Again, this only runs in Debug mode.
    /// </summary>
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
