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

        public void Log(string message)
        {
            File.WriteAllText(message, Environment.NewLine);
        }
    }
}
