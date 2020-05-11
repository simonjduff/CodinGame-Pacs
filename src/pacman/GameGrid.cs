namespace pacman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class GameGrid
    {
        private readonly Dictionary<Location, GridCell> _cells = new Dictionary<Location, GridCell>();
        private Dictionary<Location, Pellet> _pellets = new Dictionary<Location, Pellet>();
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
        }

        public bool InBounds(Location location) => _cells.ContainsKey(location);

        public void EatFood(Location location)
        {
            _pellets.Remove(location);
        }

        public short FoodValue(Location location)
        {
            if (!_pellets.ContainsKey(location))
            {
                return 0;
            }

            return _pellets[location].Value;
        }

        public void SetPellets(IEnumerable<Pellet> pellets)
        {
            _pellets = pellets.ToDictionary(k => k.Location);
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

        /// <summary>
        /// This can contain duplicates if the level wraps around.
        /// Look left, see yourself, look right, see yourself. Double count food.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEnumerable<Pellet> VisiblePelletsFrom(Location location) => VisibleTFrom(location, _pellets);
        public IEnumerable<Pac> VisibleEnemiesFrom(Location location) => VisibleTFrom(location, _enemies);

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