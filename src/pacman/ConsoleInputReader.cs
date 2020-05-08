namespace pacman
{
    using System;

    public interface IInputOutput
    {
        string ReadLine();
        void WriteLine(string text);
    }

    public class ConsoleInputOutput : IInputOutput
    {
        public string ReadLine() => Console.ReadLine();
        public void WriteLine(string text) => Console.WriteLine(text);
    }

    public class LoggingConsoleInputOutput : IInputOutput
    {
        public string ReadLine()
        {
            var line = Console.ReadLine();
            Console.Error.WriteLine(line);
            return line;
        }

        public void WriteLine(string text) => Console.WriteLine(text);
    }
}