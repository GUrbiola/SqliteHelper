using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SqliteHelper48
{
    internal static class Program
    {
        // Import Windows API to allocate a console window
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set up global exception handlers BEFORE anything else
            SetupExceptionHandlers();

            try
            {
                // Allocate console for error output
                // Try to attach to parent console first (if launched from command line)
                // If that fails, allocate a new console
                if (!AttachConsole(ATTACH_PARENT_PROCESS))
                {
                    AllocConsole();
                }

                SQLitePCL.Batteries.Init();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during initialization
                HandleUnhandledException(ex, "Application Startup");
            }
        }

        /// <summary>
        /// Sets up global exception handlers for both UI and non-UI threads
        /// </summary>
        private static void SetupExceptionHandlers()
        {
            // Handle exceptions on the UI thread
            Application.ThreadException += Application_ThreadException;

            // Handle exceptions on non-UI threads
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Catch unhandled exceptions in async operations
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        /// <summary>
        /// Handles exceptions that occur on the UI thread
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception, "UI Thread");
        }

        /// <summary>
        /// Handles exceptions that occur on non-UI threads
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                string source = e.IsTerminating ? "Non-UI Thread (Terminating)" : "Non-UI Thread";
                HandleUnhandledException(ex, source);
            }
            else
            {
                // In rare cases, the exception might not be an Exception object
                ErrorLogger.LogException(
                    new Exception($"Unknown exception object: {e.ExceptionObject?.ToString() ?? "null"}"),
                    "Non-UI Thread (Unknown Exception Type)");
            }
        }

        /// <summary>
        /// Central exception handling and logging
        /// </summary>
        private static void HandleUnhandledException(Exception ex, string source)
        {
            try
            {
                // Log the exception to file and console
                ErrorLogger.LogException(ex, source);

                // Show user-friendly error dialog
                ErrorLogger.ShowErrorDialog(ex);
            }
            catch (Exception loggingEx)
            {
                // If error handling itself fails, try one last message box
                try
                {
                    MessageBox.Show(
                        $"Critical Error: {ex.Message}\n\nAdditionally, error logging failed: {loggingEx.Message}",
                        "SqliteHelper48 - Critical Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch
                {
                    // Nothing more we can do
                }
            }
            finally
            {
                // Terminate the application
                Environment.Exit(1);
            }
        }
    }
}