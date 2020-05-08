using System;

namespace pacman
{
    public interface IInputReader
    {
        string ReadLine();
    }

    public class InputReader : IInputReader
    {
        public string ReadLine() => Console.ReadLine();
    }
}