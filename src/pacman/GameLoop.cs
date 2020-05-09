namespace pacman
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    public class GameLoop
    {
        private readonly IInputOutput _inputOutput;
        private readonly CancellationToken _cancellation;
        private readonly Dictionary<int, Pac> _myPacs = new Dictionary<int, Pac>();
        private readonly IMovementStrategy _movementStrategy;
        private readonly GameGrid _gameGrid;

        public GameLoop(IInputOutput inputOutput, 
            CancellationToken cancellation,
            IMovementStrategy movementStrategy,
            GameGrid gameGrid)
        {
            _gameGrid = gameGrid ?? throw new ArgumentNullException(nameof(gameGrid));
            _movementStrategy = movementStrategy ?? throw new ArgumentNullException(nameof(movementStrategy));
            _cancellation = cancellation;
            _inputOutput = inputOutput ?? throw new ArgumentNullException(nameof(_inputOutput));
        }

        public void Run()
        {
            while (true)
            {
                if (_cancellation.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    string[] inputs = _inputOutput.ReadLine().Split(' ');
                    int myScore = int.Parse(inputs[0]);
                    int opponentScore = int.Parse(inputs[1]);
                    int visiblePacCount = int.Parse(_inputOutput.ReadLine()); // all your pacs and enemy pacs in sight
                    Pac[] pacs = new Pac[visiblePacCount];
                    for (int i = 0; i < visiblePacCount; i++)
                    {
                        inputs = _inputOutput.ReadLine().Split(' ');
                        int pacId = int.Parse(inputs[0]); // pac number (unique within a team)
                        bool mine = inputs[1] != "0"; // true if this pac is yours
                        int x = int.Parse(inputs[2]); // position in the grid
                        int y = int.Parse(inputs[3]); // position in the grid
                        string typeId = inputs[4]; // unused in wood leagues
                        int speedTurnsLeft = int.Parse(inputs[5]); // unused in wood leagues
                        int abilityCooldown = int.Parse(inputs[6]); // unused in wood leagues
                        var location = new Location(x, y);

                        if (!mine)
                        {
                            continue;
                        }

                        if (!_myPacs.ContainsKey(pacId))
                        {
                            _myPacs.Add(pacId, new Pac(pacId, mine));
                        }

                        _myPacs[pacId].AddLocation(location);
                    }

                    int visiblePelletCount = int.Parse(_inputOutput.ReadLine()); // all pellets in sight
                    _gameGrid.SetPellets(ParsePellets(visiblePelletCount));

                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");

                    //_inputOutput.WriteLine("MOVE 0 15 10"); // MOVE <pacId> <x> <y>
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    var move = _movementStrategy.Next(_gameGrid, _myPacs.Select(p => p.Value).Single(p => p.Mine), _cancellation);
                    timer.Stop();
                    Console.Error.WriteLine($"Move time {timer.ElapsedMilliseconds}");
                    _inputOutput.WriteLine($"MOVE {move.PacId} {move.X} {move.Y}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }

            }
        }

        public IEnumerable<Pellet> ParsePellets(int pelletCount)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                var inputs = _inputOutput.ReadLine().Split(' ');
                int x = int.Parse(inputs[0]);
                int y = int.Parse(inputs[1]);
                short value = short.Parse(inputs[2]); // amount of points this pellet is worth
                yield return new Pellet(new Location(x, y), value);
            }
        }
    }
}