namespace WebBookingAPI
{
    public interface ILogger
    {
        void Log(string message, string context, string callerName = "");
    }
}
