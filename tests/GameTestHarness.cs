using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using pacman;

namespace tests
{
    public class GameTestHarness
    {
        private CancellationToken _token = new CancellationTokenSource(1500).Token;
        private string _gridInput;
        private int _gridWidth;
        private int _gridHeight;
        private readonly List<Pac> _pacs = new List<Pac>();
        private IMovementStrategy _movementStrategy;

        public GameTestHarness WithCancellationToken(CancellationToken token)
        {
            _token = token;
            return this;
        }

        public GameTestHarness WithTestGrid(int width, int height, string gridInput)
        {
            _gridHeight = height;
            _gridWidth = width;
            _gridInput = gridInput;
            return this;
        }

        public GameTestHarness WithPac(Pac pac)
        {
            _pacs.Add(pac);
            return this;
        }

        public Task RunAsync(out FakeInputOutput inputOutput)
        {
            var localInputOutput = new FakeInputOutput(_token);
            var testGrid = new TestGrid(_gridWidth, _gridHeight, _gridInput, localInputOutput);
            foreach (var pac in _pacs)
            {
                testGrid.AddPac(pac);
            }

            localInputOutput.AddInput($"{_gridWidth} {_gridHeight}");
            testGrid.WriteGrid();
            testGrid.WriteScores();
            testGrid.WritePacs();
            testGrid.WritePellets();

            var game = new Player(localInputOutput, _movementStrategy);
            var task = Task.Run(() => game.Run(_token), _token).ContinueWith(t => localInputOutput.CompleteOutput());
            inputOutput = localInputOutput;
            return task;
        }

        public GameTestHarness WithMovementStrategy(IMovementStrategy movementStrategy)
        {
            _movementStrategy = movementStrategy;
            return this;
        }
    }
}