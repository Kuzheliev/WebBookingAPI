using System.Runtime.CompilerServices;

namespace WebBookingAPI
{
    public class FileLogger : ILogger
    {
        private static FileLogger logger;

        public FileLogger() { }

        public static FileLogger Instance {
            get
            { 
               if (logger == null)
               {
                    logger = new FileLogger();
               }

               return logger;
            }
        }

        public void Log(string message, string context, string callerName = "")
        {
            // Compose file name using caller method name
        string fileName = $"{callerName}.txt";

        // Compose the content to write (message + context + newline)
        string content = $"Message: {message}{Environment.NewLine}Context: {context}{Environment.NewLine}";

        // Write content to file (overwrite if exists)
        File.WriteAllText(fileName, content);
        }
    }
}
