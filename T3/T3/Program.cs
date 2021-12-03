using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace T3
{
    public class Program
    {
        static void Main(string[] args)
        {
            PathFinder pathFinder = new PathFinder(new FileLogWritter());
            pathFinder.Find(Guid.NewGuid().ToString());

            pathFinder = new PathFinder(new ConsoleLogWritter());
            pathFinder.Find(Guid.NewGuid().ToString());

            pathFinder = new PathFinder(new SecureLogWritter(new FileLogWritter()));
            pathFinder.Find(Guid.NewGuid().ToString());

            pathFinder = new PathFinder(new SecureLogWritter(new ConsoleLogWritter()));
            pathFinder.Find(Guid.NewGuid().ToString());

            pathFinder = new PathFinder(new ConsoleLogWritter(), new SecureLogWritter(new FileLogWritter()));
            pathFinder.Find(Guid.NewGuid().ToString());
        }
    }

    public interface ILogger
    {
        void WriteError(string message);
    }

    public class PathFinder
    {
        private IEnumerable<ILogger> _loggers;

        public PathFinder(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public void Find(string message)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.WriteError(message);
            }
        }
    }

    public class ConsoleLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class FileLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    public class SecureLogWritter : ILogger
    {
        private ILogger _rootLogger;

        public SecureLogWritter(ILogger rootLogger)
        {
            _rootLogger = rootLogger;
        }

        public virtual void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _rootLogger.WriteError(message);
            }
        }
    }
}
