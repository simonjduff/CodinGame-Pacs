using System.Threading;
using System.Threading.Tasks;
using pacman;

namespace tests
{
    public abstract class PacTestBase
    {
        protected CancellationTokenSource Cancellation = new CancellationTokenSource();
        protected FakeInputOutput InputOutput;
        protected Task Awaiter;

        protected PacTestBase()
        {
            InputOutput = new FakeInputOutput(Cancellation.Token);
            var game = new Player(InputOutput);

            var cancellationToken = Cancellation.Token;
            Awaiter = Task.Run(() => game.Run(cancellationToken), cancellationToken);
            Cancellation.CancelAfter(1500);
            Awaiter.ContinueWith(t => InputOutput.CompleteOutput());
        }
    }
}