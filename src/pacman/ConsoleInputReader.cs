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
}