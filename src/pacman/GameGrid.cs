namespace pacman
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    public class GameGrid
    {
        private readonly Dictionary<Location, GridCell> _cells = new Dictionary<Location, GridCell>();
        private Dictionary<Location, Pellet> _visiblePellets = new Dictionary<Location, Pellet>();
        private Dictionary<Location, Pellet> _bigPellets = new Dictionary<Location, Pellet>();
        public short Width = 0;
        public short Height = 0;
        private Dictionary<Location, Pac> _enemies;
        private static readonly Random Random = new Random();

        public GridCell this[Location location] => _cells[location];

        public void StoreGrid(IEnumerable<string> gridLines)
        {
            short row = 0;
            foreach (string line in gridLines)
            {
                Height++;
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                for (short cell = 0; cell < line.Length; cell++)
                {
                    if (row == 0)
                    {
                        Width = (short)line.Length;
                    }

                    var location = new Location(cell, row);
                    try
                    {
                        switch (line[cell])
                        {
                            case '#':
                                _cells.Add(location, new GridCell(false, location));
                                continue;
                            case ' ':
                                _cells.Add(location, new GridCell(true, location));
                                continue;
                            default:
                                throw new InvalidOperationException($"Unrecognized cell type {line[cell]}");
                        }
                    }
                    catch (ArgumentException e)
                    {
                        throw new Exception($"Argument exception at location {location}", e);
                    }
                }

                row++;
            }

            foreach (var cell in _cells)
            {

            }
        }

        public short FoodValue(Location location)
        {
            if (!_visiblePellets.ContainsKey(location))
            {
                return 0;
            }

            return _visiblePellets[location].Value;
        }

        public void SetPellets(IEnumerable<Pellet> pellets)
        {
            _visiblePellets = new Dictionary<Location, Pellet>();
            var localBigPellets = _bigPellets;
            _bigPellets = new Dictionary<Location, Pellet>();
            foreach (var pellet in pellets)
            {
                _visiblePellets.Add(pellet.Location, pellet);
                var cell = _cells[pellet.Location];
                cell.PossiblePelletValue = pellet.Value;
                if (pellet.Value == 10)
                {
                    _bigPellets.Add(pellet.Location, pellet);
                }
            }

            foreach (var pellet in localBigPellets)
            {
                if (!_bigPellets.ContainsKey(pellet.Key))
                {
                    Console.Error.WriteLine($"Big pellet at {pellet.Key} has been eaten!");
                    var cell = _cells[pellet.Key];
                    cell.PossiblePelletValue = 0;
                }
            }
        }

        public void SetEnemies(IEnumerable<Pac> enemies)
        {
            _enemies = new Dictionary<Location, Pac>();
            foreach (var enemy in enemies)
            {
                if (_enemies.ContainsKey(enemy.Location))
                {
                    Console.Error.WriteLine($"Two enemies in same space. {enemy.Id} and {_enemies[enemy.Location].Id}");
                    throw new InvalidOperationException("Two enemies in same space");
                }

                _enemies.Add(enemy.Location, enemy);
            }
        }

        public Location RandomLocation
        {
            get
            {
                while (true)
                {
                    var randomLocation = new Location((short)Random.Next(0, Width), (short)Random.Next(0, Height));
                    if (_cells[randomLocation].Traversable)
                    {
                        return randomLocation;
                    }
                }
            }
        }

        public IEnumerable<Pellet> VisiblePelletsFrom(Location location)
        {
            return VisibleTFrom(location, _visiblePellets);
        }

        public void ClearEmptyPelletsVisibleFromLocation(Location location)
        {
            var visibleCells = VisibleTFrom<GridCell>(location, _cells);
            foreach (var cell in visibleCells)
            {
                if (!_visiblePellets.ContainsKey(cell.Location))
                {
                    cell.PossiblePelletValue = 0;
                }
            }
        }

        public IEnumerable<Pac> VisibleEnemiesFrom(Location location)
        {
            _cells[location].PossiblePelletValue = 0;
            return VisibleTFrom(location, _enemies);
        }

        public GridCell West(Location origin)
        {
            var newX = (short)(origin.X - 1);
            if (newX < 0)
            {
                newX = (short)(Width - 1);
            }

            var newLocation = new Location(newX, origin.Y);
            if (_cells.ContainsKey(newLocation))
            {
                return _cells[newLocation];
            }

            return new GridCell(false, newLocation);
        }

        public GridCell East(Location origin)
        {
            var newX = (short)(origin.X + 1);
            if (newX >= Width)
            {
                newX = (short)(0);
            }

            var newLocation = new Location(newX, origin.Y);
            if (_cells.ContainsKey(newLocation))
            {
                return _cells[newLocation];
            }

            return new GridCell(false, newLocation);
    }
        public GridCell South(Location origin)
        {
            var newY = (short)(origin.Y + 1);
            if (newY >= Height)
            {
                newY = (short)(0);
            }

            var newLocation = new Location(origin.X, newY);
            if (_cells.ContainsKey(newLocation))
            {
                return _cells[newLocation];
            }

            return new GridCell(false, newLocation);
        }
        public GridCell North(Location origin)
        {
            var newY = (short)(origin.Y - 1);
            if (newY < 0)
            {
                newY = (short)(Height - 1);
            }

            var newLocation = new Location(origin.X, newY);
            if (_cells.ContainsKey(newLocation))
            {
                return _cells[newLocation];
            }

            return new GridCell(false, newLocation);
        }

        public short NeighbourCount(Location origin)
        {
            Func<Location, Func<Location, GridCell>, short> t = (c,f) => (short)(f(c).Traversable ? 1 : 0);
            var edges = new Func<Location,GridCell>[] {North, South, East, West};

            return (short)edges.Sum(e => t(origin, e));
        }

        private IEnumerable<T> VisibleTFrom<T>(Location location, IDictionary<Location, T> dictionary)
        {
            Func<Location, GridCell>[] searches =
            {
                North,
                South,
                East,
                West,
            };

            List<Location> visited = new List<Location>();

            foreach (var search in searches)
            {
                var next = search(location);
                while (next.Traversable && !visited.Contains(next.Location))
                {
                    visited.Add(next.Location);

                    // Clean up the pellets if none are visible
                    if (next.PossiblePelletValue > 0 && !_visiblePellets.ContainsKey(next.Location))
                    {
                        next.PossiblePelletValue = 0;
                    }

                    if (dictionary.ContainsKey(next.Location))
                    {
                        yield return dictionary[next.Location];
                    }

                    next = search(next.Location);
                }
            }
        }
    }
}