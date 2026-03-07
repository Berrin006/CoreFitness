using Domain.Abstractions.Logging;
using System.Diagnostics;

namespace Infrastructure.Logging;

public sealed class Logger : ILogger
{
    private readonly string _logFilePath;

    public Logger()
    {
        var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        Directory.CreateDirectory(logDirectory);

        _logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyy-MM-dd}.txt");
    }

    public void Log(string message)
    {
        var formatted = $"{DateTime.Now:0} | {message}";

        Console.WriteLine( formatted );
        Debug.WriteLine( formatted );

        File.AppendAllText(_logFilePath, formatted + Environment.NewLine);
    }

    public void Log(Exception exception)
    {
        if(exception == null) 
            return;

        Log(exception.ToString());
    }
}
