using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SqliteHelper48
{
    /// <summary>
    /// Handles logging of unhandled exceptions to file and console
    /// </summary>
    public static class ErrorLogger
    {
        private static string _logFilePath;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Gets the path to the log file in the temp directory
        /// </summary>
        public static string LogFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_logFilePath))
                {
                    string tempPath = Path.GetTempPath();
                    string logFileName = $"SqliteHelper48_Error_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                    _logFilePath = Path.Combine(tempPath, logFileName);
                }
                return _logFilePath;
            }
        }

        /// <summary>
        /// Logs an unhandled exception with full details
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="source">Source of the exception (e.g., "UI Thread", "Background Thread")</param>
        public static void LogException(Exception ex, string source = "Unknown")
        {
            try
            {
                string logMessage = BuildDetailedLogMessage(ex, source);

                // Write to file
                WriteToFile(logMessage);

                // Write to console
                WriteToConsole(logMessage);
            }
            catch (Exception loggingEx)
            {
                // Last resort - try to show a message box if logging itself fails
                try
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Critical Error: Failed to log exception.\n\nOriginal Error: {ex.Message}\n\nLogging Error: {loggingEx.Message}",
                        "SqliteHelper48 - Critical Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                catch
                {
                    // If even message box fails, we're in deep trouble
                    // Nothing more we can do
                }
            }
        }

        /// <summary>
        /// Builds a detailed log message from an exception
        /// </summary>
        private static string BuildDetailedLogMessage(Exception ex, string source)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("================================================================================");
            sb.AppendLine($"UNHANDLED EXCEPTION - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("================================================================================");
            sb.AppendLine();
            sb.AppendLine($"Source: {source}");
            sb.AppendLine($"Application: SqliteHelper48");
            sb.AppendLine($"Version: {GetApplicationVersion()}");
            sb.AppendLine($"Operating System: {Environment.OSVersion}");
            sb.AppendLine($".NET Framework: {Environment.Version}");
            sb.AppendLine($"Machine Name: {Environment.MachineName}");
            sb.AppendLine($"User: {Environment.UserName}");
            sb.AppendLine($"Working Directory: {Environment.CurrentDirectory}");
            sb.AppendLine();

            // Log loaded assemblies (especially important for BadImageFormatException)
            if (ex is BadImageFormatException || ex is FileLoadException || ex is FileNotFoundException)
            {
                AppendLoadedAssemblies(sb);
            }

            // Log the main exception
            AppendExceptionDetails(sb, ex, "EXCEPTION");

            // Log inner exceptions if any
            if (ex.InnerException != null)
            {
                sb.AppendLine();
                int innerCount = 1;
                Exception innerEx = ex.InnerException;

                while (innerEx != null)
                {
                    AppendExceptionDetails(sb, innerEx, $"INNER EXCEPTION #{innerCount}");
                    innerEx = innerEx.InnerException;
                    innerCount++;
                }
            }

            sb.AppendLine();
            sb.AppendLine("================================================================================");
            sb.AppendLine("END OF ERROR LOG");
            sb.AppendLine("================================================================================");
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// Appends detailed information about an exception to the string builder
        /// </summary>
        private static void AppendExceptionDetails(StringBuilder sb, Exception ex, string title)
        {
            sb.AppendLine($"--- {title} ---");
            sb.AppendLine();

            sb.AppendLine($"Exception Type: {ex.GetType().FullName}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"HRESULT: 0x{ex.HResult:X8}");
            sb.AppendLine($"Source: {ex.Source ?? "N/A"}");
            sb.AppendLine($"Help Link: {ex.HelpLink ?? "N/A"}");
            sb.AppendLine();

            // Extract method, class, file, and line information from stack trace
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                ExtractStackTraceDetails(sb, ex);
            }
            else
            {
                sb.AppendLine("Stack Trace: [No stack trace available]");
            }

            // Additional data if available
            if (ex.Data.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Additional Data:");
                foreach (System.Collections.DictionaryEntry entry in ex.Data)
                {
                    sb.AppendLine($"  {entry.Key}: {entry.Value}");
                }
            }
        }

        /// <summary>
        /// Extracts and formats detailed stack trace information
        /// </summary>
        private static void ExtractStackTraceDetails(StringBuilder sb, Exception ex)
        {
            StackTrace stackTrace = new StackTrace(ex, true);

            if (stackTrace.FrameCount > 0)
            {
                // Get the top frame (where the exception occurred)
                StackFrame topFrame = stackTrace.GetFrame(0);

                if (topFrame != null)
                {
                    sb.AppendLine("Location Information:");

                    var method = topFrame.GetMethod();
                    if (method != null)
                    {
                        sb.AppendLine($"  Method: {method.Name}");
                        sb.AppendLine($"  Class: {method.DeclaringType?.FullName ?? "N/A"}");
                    }

                    string fileName = topFrame.GetFileName();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        sb.AppendLine($"  File: {fileName}");
                        sb.AppendLine($"  Line: {topFrame.GetFileLineNumber()}");
                        sb.AppendLine($"  Column: {topFrame.GetFileColumnNumber()}");
                    }
                    else
                    {
                        sb.AppendLine($"  File: [Not available - may be release build without debug symbols]");
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine("Full Stack Trace:");

            // Split stack trace by lines and indent each line
            string[] stackLines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in stackLines)
            {
                sb.AppendLine($"  {line.Trim()}");
            }
        }

        /// <summary>
        /// Writes the log message to a file in the temp directory
        /// </summary>
        private static void WriteToFile(string message)
        {
            lock (_lockObject)
            {
                try
                {
                    // Ensure the directory exists
                    string directory = Path.GetDirectoryName(LogFilePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Append to the log file (or create if it doesn't exist)
                    File.AppendAllText(LogFilePath, message, Encoding.UTF8);

                    // Add a note to console about where the file was saved
                    Console.WriteLine($"[ERROR LOG WRITTEN TO FILE: {LogFilePath}]");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FAILED TO WRITE LOG FILE: {ex.Message}]");
                }
            }
        }

        /// <summary>
        /// Writes the log message to the console
        /// </summary>
        private static void WriteToConsole(string message)
        {
            try
            {
                // Change console color to red for errors
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(message);

                // Restore original color
                Console.ForegroundColor = originalColor;
            }
            catch
            {
                // If console writing fails, just ignore it
                // (console might not be available in Windows Forms app)
            }
        }

        /// <summary>
        /// Gets the application version
        /// </summary>
        private static string GetApplicationVersion()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return version?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Appends information about all loaded assemblies to help diagnose assembly loading issues
        /// </summary>
        private static void AppendLoadedAssemblies(StringBuilder sb)
        {
            try
            {
                sb.AppendLine("--- LOADED ASSEMBLIES ---");
                sb.AppendLine();
                sb.AppendLine("Critical System Assemblies:");

                // Focus on the assemblies that commonly cause BadImageFormatException
                string[] criticalAssemblies = new[]
                {
                    "System.ValueTuple",
                    "System.Buffers",
                    "System.Memory",
                    "System.Runtime.CompilerServices.Unsafe",
                    "System.Numerics.Vectors",
                    "SQLitePCLRaw.batteries_v2",
                    "SQLitePCLRaw.core",
                    "SQLitePCLRaw.provider.dynamic_cdecl"
                };

                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .OrderBy(a => a.FullName)
                    .ToList();

                foreach (var criticalName in criticalAssemblies)
                {
                    var assembly = loadedAssemblies.FirstOrDefault(a => a.GetName().Name == criticalName);
                    if (assembly != null)
                    {
                        sb.AppendLine($"  {criticalName}:");
                        sb.AppendLine($"    Version: {assembly.GetName().Version}");
                        sb.AppendLine($"    Location: {(string.IsNullOrEmpty(assembly.Location) ? "[Dynamic/In-Memory]" : assembly.Location)}");

                        // Check if it's a reference assembly
                        try
                        {
                            var refAssemblyAttr = assembly.GetCustomAttributes(typeof(System.Runtime.CompilerServices.ReferenceAssemblyAttribute), false);
                            if (refAssemblyAttr != null && refAssemblyAttr.Length > 0)
                            {
                                sb.AppendLine($"    WARNING: This is a REFERENCE ASSEMBLY (cannot be executed!)");
                            }
                            else
                            {
                                sb.AppendLine($"    Type: Runtime Assembly (OK)");
                            }
                        }
                        catch
                        {
                            sb.AppendLine($"    Type: Unknown");
                        }

                        // Show file size if available
                        if (!string.IsNullOrEmpty(assembly.Location) && File.Exists(assembly.Location))
                        {
                            var fileInfo = new FileInfo(assembly.Location);
                            sb.AppendLine($"    File Size: {fileInfo.Length:N0} bytes");
                        }
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine($"  {criticalName}: [NOT LOADED]");
                        sb.AppendLine();
                    }
                }

                sb.AppendLine("All Loaded Assemblies:");
                foreach (var assembly in loadedAssemblies)
                {
                    sb.AppendLine($"  {assembly.FullName}");
                    if (!string.IsNullOrEmpty(assembly.Location))
                    {
                        sb.AppendLine($"    Location: {assembly.Location}");
                    }
                }

                sb.AppendLine();
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Failed to retrieve loaded assemblies: {ex.Message}");
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Shows a user-friendly error dialog with option to view log
        /// </summary>
        public static void ShowErrorDialog(Exception ex)
        {
            try
            {
                string message = $"SqliteHelper48 has encountered an unexpected error and needs to close.\n\n" +
                                $"Error Type: {ex.GetType().Name}\n" +
                                $"Message: {ex.Message}\n\n" +
                                $"A detailed error log has been saved to:\n{LogFilePath}\n\n" +
                                $"Would you like to open the log file?";

                var result = System.Windows.Forms.MessageBox.Show(
                    message,
                    "SqliteHelper48 - Unhandled Exception",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Error);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // Open the log file in the default text editor
                    Process.Start("notepad.exe", LogFilePath);
                }
            }
            catch
            {
                // If dialog fails, try a simple message box
                try
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Critical Error: {ex.Message}\n\nLog file: {LogFilePath}",
                        "SqliteHelper48 - Critical Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                catch
                {
                    // Nothing more we can do
                }
            }
        }
    }
}
