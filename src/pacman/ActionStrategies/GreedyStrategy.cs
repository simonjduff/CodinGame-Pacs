namespace pacman.ActionStrategies
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using System;
    public class GreedyStrategy : IActionStrategy
    {
        private readonly GameGrid _grid;
        //private readonly IDictionary<PacKey,Queue<Location>> _paths = new Dictionary<PacKey, Queue<Location>>();
        private readonly IDictionary<PacKey, Location> _randomDestinations = new Dictionary<PacKey, Location>();

        public GreedyStrategy(GameGrid grid)
        {
            _grid = grid;
        }

        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            if (pac.SpecialActionReady)
            {
                return new SpeedAction(pac);
            }

            //if (_paths.ContainsKey(pac.Key) && _paths[pac.Key].TryDequeue(out var next))
            //{
            //    return new MoveAction(pac, next);
            //}

            Queue<Node> queue = new Queue<Node>();
            var startNode = new Node(pac.Location, Enumerable.Empty<Location>(), 0);
            queue.Enqueue(startNode);
            Node highestValue = startNode;

            while (queue.TryDequeue(out Node current))
            {
                if (current.Value > highestValue.Value)
                {
                    highestValue = current;
                }

                if (current.Path.Count > 5)
                {
                    break;
                }

                EnqueueIfValid(queue, _grid.North(current.Location), current);
                EnqueueIfValid(queue, _grid.South(current.Location), current);
                EnqueueIfValid(queue, _grid.East(current.Location), current);
                EnqueueIfValid(queue, _grid.West(current.Location), current);
            }

            if (highestValue.Path.Count == 0)
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
            Console.Error.WriteLine($"Pac {pac.Id} Highest value {highestValue.Value} at {highestValue.Location}. Path {pathString}");

            //_paths[pac.Key] = new Queue<Location>(10);
            //foreach (var item in highestValue.Path.Skip(1).Take(10))
            //{
            //    _paths[pac.Key].Enqueue(item);
            //}

            //return new MoveAction(pac, _paths[pac.Key].Dequeue());

            var neighbours = _grid.NeighbourCount(highestValue.Path[1]);
            if (neighbours == 1)
            {
                return new MoveAction(pac, highestValue.Path[1]);
            }

            return new MoveAction(pac, highestValue.Path[2]);
        }

        private void EnqueueIfValid(Queue<Node> queue, Location cell, Node start)
        {
            if (_grid.Traversable(cell) && start.Path.Count(p => p == cell) < 2)
            {
                var path = start.Path.Append(start.Location).ToList();

                int value = path.Sum(p => start.Path.Contains(cell) ? 0 : _grid.FoodValue(p));

                queue.Enqueue(new Node(cell, path, value));
            }
        }

        public class Node
        {
            public Node(Location location, 
                IEnumerable<Location> path,
                int value)
            {
                Location = location;
                Path = path.ToList();
                Value = value;
            }
            public Location Location { get; }
            public List<Location> Path { get; }
            public int Value { get; }
        }
    }
}