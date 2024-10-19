using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

public enum LogLevel
{
    INFO,
    WARNING,
    ERROR
}

public class Logger
{
    private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());
    private LogLevel currentLogLevel;
    private string logFilePath;

    private Logger()
    {
        LoadConfiguration();
    }

    public static Logger GetInstance()
    {
        return instance.Value;
    }

    public void LogMessage(string message, LogLevel level)
    {
        if (level >= currentLogLevel)
        {
            string logEntry = $"{DateTime.Now} [{level}] {message}";
            lock (this)
            {
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
        }
    }

    public void SetLogLevel(LogLevel level)
    {
        currentLogLevel = level;
    }

    private void LoadConfiguration()
    {
        try
        {
            if (!File.Exists("config.json"))
            {
                throw new FileNotFoundException("Конфигурационный файл не найден.");
            }

            string json = File.ReadAllText("config.json");
            dynamic config = JsonConvert.DeserializeObject(json);

            if (config.LogLevel == null || config.LogFilePath == null)
            {
                throw new ArgumentNullException("Один или несколько параметров конфигурации отсутствуют.");
            }

            currentLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), (string)config.LogLevel);
            logFilePath = (string)config.LogFilePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке конфигурации: {ex.Message}");
        
            currentLogLevel = LogLevel.INFO;
            logFilePath = "log.txt";
        }
    }
}

public class LogReader
{
    private string logFilePath;

    public LogReader(string path)
    {
        logFilePath = path;
    }

    public void ReadLogs(LogLevel level)
    {
        var lines = File.ReadAllLines(logFilePath);
        foreach (var line in lines)
        {
            if (line.Contains($"[{level}]"))
            {
                Console.WriteLine(line);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Logger logger = Logger.GetInstance();
        logger.SetLogLevel(LogLevel.INFO);

        Thread[] threads = new Thread[3];

        for (int i = 0; i < threads.Length; i++)
        {
            int threadId = i + 1;
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 4; j++)
                {
                    logger.LogMessage($"Сообщение {j} (ошибка) от потока {threadId}", LogLevel.INFO);

                    if (j == 1)
                    {
                        logger.LogMessage($"Сообщение {j} предупреждение от потока {threadId}", LogLevel.WARNING);
                    }
                    if (j == 2)
                    {
                        logger.LogMessage($"Сообщение {j} (ошибка) от потока {threadId}", LogLevel.ERROR);
                    }

                    Thread.Sleep(100); 
                }
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        // Чтение логов
        LogReader logReader = new LogReader("log.txt");
        Console.WriteLine("Логи уровня INFO:");
        logReader.ReadLogs(LogLevel.INFO);

        Console.WriteLine("Логи уровня WARNING:");
        logReader.ReadLogs(LogLevel.WARNING);

        Console.WriteLine("Логи уровня ERROR:");
        logReader.ReadLogs(LogLevel.ERROR);
    }
}

