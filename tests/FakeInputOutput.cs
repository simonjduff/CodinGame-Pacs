using System;
using System.Collections.Concurrent;
using pacman;

namespace tests
{
    public class FakeInputOutput : IInputOutput, IDisposable
    {
        private readonly BlockingCollection<string> _input = new BlockingCollection<string>();
        private readonly BlockingCollection<string> _output = new BlockingCollection<string>();

        public void AddInput(string input)
        {
            _input.Add(input);
        }

        public string ReadOutput() => _output.Take();

        public string ReadLine() => _input.Take();

        public void WriteLine(string text)
        {
            _output.Add(text);
        }

        public void Dispose()
        {
            _input?.Dispose();
            _output?.Dispose();
        }
    }
}