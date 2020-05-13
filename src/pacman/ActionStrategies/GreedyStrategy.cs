namespace pacman.ActionStrategies
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using System;
    public class GreedyStrategy : IActionStrategy
    {
        private readonly GameGrid _grid;

        private readonly IDictionary<PacKey, Location> _randomDestinations = new Dictionary<PacKey, Location>();

        public GreedyStrategy(GameGrid grid)
        {
            _grid = grid;
        }

        public NextAction Next(Pac pac, CancellationToken receivedCancellation)
        {
            var innerCancellation = new CancellationTokenSource(10);
            var cancellation =  CancellationTokenSource.CreateLinkedTokenSource(receivedCancellation, innerCancellation.Token);

            if (pac.SpecialActionReady)
            {
                return new SpeedAction(pac);
            }

            //if (_paths.ContainsKey(pac.Key) && _paths[pac.Key].TryDequeue(out var next))
            //{
            //    return new MoveAction(pac, next);
            //}

            Queue<Node> queue = new Queue<Node>();
            var startNode = new Node(pac.Location, new Location[0], 0);
            queue.Enqueue(startNode);
            Node highestValue = startNode;

            while (queue.TryDequeue(out Node current))
            {
                if (cancellation.IsCancellationRequested)
                {
                    Console.Error.WriteLine("TIMEOUT CANCELLED");
                    break;
                }

                if (current.Value > highestValue.Value)
                {
                    highestValue = current;
                }

                if (current.Path.Length > 7)
                {
                    continue;
                }

                current.Append(current.Location);

                EnqueueIfValid(queue, _grid.North(current.Location), current);
                EnqueueIfValid(queue, _grid.South(current.Location), current);
                EnqueueIfValid(queue, _grid.East(current.Location), current);
                EnqueueIfValid(queue, _grid.West(current.Location), current);
            }

            if (highestValue.Path.Length < 2)
            {
                Console.Error.WriteLine($"{pac.Id} No movement found");

                if (!_randomDestinations.ContainsKey(pac.Key) || _randomDestinations[pac.Key] == pac.Location)
                {
                    _randomDestinations[pac.Key] = _grid.RandomLocation;
                }

                return new MoveAction(pac, _randomDestinations[pac.Key]);
            }

            if (_randomDestinations.ContainsKey(pac.Key))
            {
                _randomDestinations.Remove(pac.Key);
            }

            var pathString = string.Join("|", highestValue.Path.Select(p => $"{p} ({_grid.FoodValue(p)})"));
            Console.Error.WriteLine($"Pac {pac.Id} at {pac.Location} Highest value {highestValue.Value} at {highestValue.Location}. Path {pathString}");

            //_paths[pac.Key] = new Queue<Location>(10);
            //foreach (var item in highestValue.Path.Skip(1).Take(10))
            //{
            //    _paths[pac.Key].Enqueue(item);
            //}

            //return new MoveAction(pac, _paths[pac.Key].Dequeue());

            if (highestValue.Path.Length == 2)
            {
                return new MoveAction(pac, highestValue.Path[1]);
            }

            if (highestValue.Path[2] == highestValue.Path[0])
            {
                Console.Error.WriteLine($"Pac {pac.Id}. Backtracking 1, moving 1 space only");
                return new MoveAction(pac, highestValue.Path[1]);
            }

            return new MoveAction(pac, highestValue.Path[2]);
        }

        private void EnqueueIfValid(Queue<Node> queue, Location cell, Node start)
        {
            // Ignore the cell we're currently standing on
            var traversable = cell == start.Path[0] || _grid.Traversable(cell);

            if (traversable && start.Path.Count(p => p == cell) <= 2)
            {
                int value = start.Path.Sum(p => start.Path.Contains(cell) ? 0 : _grid.FoodValue(p));

                queue.Enqueue(new Node(cell, start.Path, value));
            }
        }

        public class Node
        {
            public Node(Location location, 
                Location[] path,
                int value)
            {
                Location = location;
                Path = path;
                Value = value;
            }
            public Location Location { get; }
            public Location[] Path { get; private set; }
            public int Value { get; }

            public void Append(Location location)
            {
                var old = Path;
                Path = new Location[Path.Length + 1];
                old.CopyTo(Path, 0);
                Path[^1] = location;
            }

            public override string ToString()
            {
                return string.Join("|", Path);
            }
        }
    }
}