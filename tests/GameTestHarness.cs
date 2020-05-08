using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using pacman;

namespace tests
{
    public class GameTestHarness
    {
        public FakeInputOutput InputOutput;
        private readonly CancellationToken _cancellationToken;

        public GameTestHarness() : this(new CancellationTokenSource(1500).Token)
        {
        }

        public GameTestHarness(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            InputOutput = new FakeInputOutput(cancellationToken);
        }

        public Task RunAsync()
        {
            var game = new Player(InputOutput);

            var gameTask = Task.Run(() => game.Run(_cancellationToken), _cancellationToken);
            // ReSharper disable once MethodSupportsCancellation
            return gameTask.ContinueWith(t => InputOutput.CompleteOutput());
        }
    }
}