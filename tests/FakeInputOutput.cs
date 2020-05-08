using System;
using System.Collections.Concurrent;
using System.Threading;
using pacman;

namespace tests
{
    public class FakeInputOutput : IInputOutput, IDisposable
    {
        private readonly BlockingCollection<string> _input = new BlockingCollection<string>();
        private readonly BlockingCollection<string> _output = new BlockingCollection<string>();
        private readonly CancellationToken _cancellation;

        public FakeInputOutput(CancellationToken cancellation)
        {
            _cancellation = cancellation;
        }

        public void AddInput(string input)
        {
            _input.Add(input);
        }

        public string ReadOutput() => _output.Take();
        public void CompleteOutput() => _output.CompleteAdding();
        public bool CanReadOutput => !_output.IsCompleted;

        public string ReadLine() => _input.Take(_cancellation);

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