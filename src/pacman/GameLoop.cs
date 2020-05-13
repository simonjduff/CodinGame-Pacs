namespace pacman
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using pacman.ActionStrategies;
    public class GameLoop
    {
        private readonly IInputOutput _inputOutput;
        private readonly CancellationToken _cancellation;
        private readonly Dictionary<PacKey, Pac> _pacs = new Dictionary<PacKey, Pac>();
        private readonly IActionStrategy _actionStrategy;
        private readonly GameGrid _gameGrid;

        public GameLoop(IInputOutput inputOutput, 
            CancellationToken cancellation,
            IActionStrategy actionStrategy,
            GameGrid gameGrid)
        {
            _gameGrid = gameGrid ?? throw new ArgumentNullException(nameof(gameGrid));
            _actionStrategy = actionStrategy ?? throw new ArgumentNullException(nameof(actionStrategy));
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
                    List<PacKey> seenKeys = new List<PacKey>();
                    for (int i = 0; i < visiblePacCount; i++)
                    {
                        var line = _inputOutput.ReadLine();
                        Console.Error.WriteLine($"Pac line {line}");
                        inputs = line.Split(' ');
                        int pacId = int.Parse(inputs[0]); // pac number (unique within a team)
                        bool mine = inputs[1] != "0"; // true if this pac is yours
                        short x = short.Parse(inputs[2]); // position in the grid
                        short y = short.Parse(inputs[3]); // position in the grid
                        string typeId = inputs[4]; // unused in wood leagues
                        short speedTurnsLeft = short.Parse(inputs[5]); // unused in wood leagues
                        short abilityCooldown = short.Parse(inputs[6]); // unused in wood leagues
                        var location = new Location(x, y);

                        Pac pac;

                        var key = new PacKey(pacId, mine);
                        seenKeys.Add(key);
                        if (!_pacs.ContainsKey(key))
                        {
                            _pacs.Add(key, new Pac(pacId, mine, _actionStrategy, new GiveWayMovementStrategy(_gameGrid)));
                        }

                        pac = _pacs[key];

                        pac.AddLocation(location);
                        pac.AbilityCooldown = abilityCooldown;
                        pac.SpeedTurnsLeft = speedTurnsLeft;
                        pac.Type = typeId;

                        //_gameGrid.VisiblePelletsFrom(pac.Location);
                    }

                    var deletion = _pacs.Select(p => p.Key).Where(p => !seenKeys.Contains(p));
                    foreach (var d in deletion)
                    {
                        _pacs.Remove(d);
                    }

                    int visiblePelletCount = int.Parse(_inputOutput.ReadLine()); // all pellets in sight
                    _gameGrid.SetPellets(ParsePellets(visiblePelletCount));
                    _gameGrid.SetEnemies(_pacs.Values.Where(p => !p.Mine));
                    _gameGrid.SetMyPacs(_pacs.Values.Where(p => p.Mine && p.Type != PacType.Dead));
                    //Console.Error.WriteLine(_gameGrid.ToString());

                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");

                    //_inputOutput.WriteLine("MOVE 0 15 10"); // MOVE <pacId> <x> <y>

                    var myPacs = _pacs.Values.Where(p => p.Mine);

                    var nextActions = myPacs.Select(pac => pac.NextAction(_gameGrid, _cancellation)).ToDictionary(p => p.Pac.Key);

                    var collisions =
                        nextActions.Values.Where(n => n is MoveAction).GroupBy(g => ((MoveAction) g).Location).Where(g => g.Count() > 1);
                    foreach (var collision in collisions)
                    {
                        var giveWayer = collision.First().Pac;
                        nextActions[giveWayer.Key] = giveWayer.GiveWay(_cancellation);
                    }
                    

                    var moves = string.Join("|", nextActions.Values);
                    _inputOutput.WriteLine(moves);
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
                short x = short.Parse(inputs[0]);
                short y = short.Parse(inputs[1]);
                short value = short.Parse(inputs[2]); // amount of points this pellet is worth

                yield return new Pellet(new Location(x, y), value);
            }
        }
    }
}