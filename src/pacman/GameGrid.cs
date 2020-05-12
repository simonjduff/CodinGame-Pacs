namespace pacman
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    public class GameGrid
    {
        private ulong[] _grid = new ulong[0];

        private ulong[] _possibleSmallPellets = new ulong[0];
        private ulong[] _visibleSmallPellets = new ulong[0];
        private ulong[] _bigPellets = new ulong[0];
        public short Width = 0;
        public short Height = 0;
        private readonly Dictionary<Location, Pac> _enemies = new Dictionary<Location, Pac>();
        private readonly Dictionary<Location, Pac> _myPacs = new Dictionary<Location, Pac>();
        private static readonly Random Random = new Random();

        public string MaskToString(ulong[] fullMask)
        {
            char[][] grid = new char[Height][];
            for (short row = 0; row < Height; row++)
            {
                grid[row] = new char[Width];
            }

            for (short cell = 0; cell < Width; cell++)
            {
                var mask = (ulong) Math.Pow(2, Width - cell - 1);
                for (short row = 0; row < fullMask.Length; row++)
                {
                    if ((mask & fullMask[row]) == mask)
                    {
                        grid[row][cell] = '.';
                        continue;
                    }

                    grid[row][cell] = ' ';
                }
            }

            return string.Join(Environment.NewLine, grid.Select(r => new string(r)));
        }

        public override string ToString()
        {
            char[][] grid = new char[Height][];
            for (short row = 0; row < Height; row++)
            {
                grid[row] = new char[Width];
            }

            for (short cell = 0; cell < Width; cell++)
            {
                var mask = (ulong)Math.Pow(2, Width - cell - 1);
                for (short row = 0; row < _grid.Length; row++)
                {
                    if ((mask & _possibleSmallPellets[row]) == mask)
                    {
                        grid[row][cell] = '.';
                    }
                    else if ((mask & _bigPellets[row]) == mask)
                    {
                        grid[row][cell] = 'X';
                    }
                    else if (_myPacs.ContainsKey(new Location(cell, row)))
                    {
                        grid[row][cell] = 'M';
                    }
                    else if (_enemies.ContainsKey(new Location(cell, row)))
                    {
                        grid[row][cell] = 'E';
                    }
                    else if ((mask & _grid[row]) == mask)
                    {
                        grid[row][cell] = ' ';
                    }
                    else
                    {
                        grid[row][cell] = '#';
                    }
                }
            }

            return string.Join(Environment.NewLine, grid.Select(r => new string(r)));
        }

        public void StoreGrid(IEnumerable<string> gridLines)
        {
            Console.Error.WriteLine("Storing the original grid");
            var lines = gridLines.Where(l => !string.IsNullOrEmpty(l)).ToArray();
            Height = (short)lines.Length;
            Width = (short)lines[0].Length;

            // Set up the arrays of the appropriate size
            _grid = new ulong[Height];
            _visibleSmallPellets = new ulong[Height];
            _possibleSmallPellets = new ulong[Height];
            _bigPellets = new ulong[Height];

            // Store the grid itself.
            // 1 represents a traversable cell
            for (int row = 0; row < Height; row++)
            {
                _grid[row] = 0;
                for (int cell = 0; cell < Width; cell++)
                {
                    if (lines[row][cell] == ' ')
                    {
                        ulong mask = (ulong)Math.Pow(2, Width - cell - 1);
                        _grid[row] = mask | _grid[row];
                    }
                }
            }

            // Set possible small pellets on all traversable squares
            // 1 represents a possible small pellet
            _grid.CopyTo(_possibleSmallPellets, 0);
        }

        public short FoodValue(Location location)
        {
            int row = location.GridRow;
            ulong mask = location.GridMask(Width);

            var value = (short)(((_possibleSmallPellets[row] & mask) == mask ? 1 : 0) +
                            ((_bigPellets[row] & mask) == mask ? 10 : 0));
            return value;
        }

        public void SetPellets(IEnumerable<Pellet> pellets)
        {
            _visibleSmallPellets = new ulong[Height];
            foreach (var pellet in pellets)
            {
                var row = pellet.Location.GridRow;
                switch (pellet.Value)
                {
                    case 1:
                        _visibleSmallPellets[row] =
                            _visibleSmallPellets[row] | pellet.Location.GridMask(Width);
                        break;
                    case 10:
                        _bigPellets[row] =
                            _bigPellets[row] | pellet.Location.GridMask(Width);
                        // Flip the possible small to 0, as we know there can't be a small pellet here
                        ClearPossiblePelletAt(pellet.Location);
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected pellet value {pellet.Value}");
                }
            }
        }

        public void SetMyPacs(IEnumerable<Pac> myPacs)
        {
            _myPacs.Clear();
            foreach (var pac in myPacs)
            {
                ClearPossiblePelletAt(pac.Location);
                ClearEmptyPelletsVisibleFromLocation(pac.Location);
                _myPacs[pac.Location] = pac;
            }
        }

        public void SetEnemies(IEnumerable<Pac> enemies)
        {
            _enemies.Clear();
            foreach (var enemy in enemies)
            {
                ClearPossiblePelletAt(enemy.Location);
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
                    if (Traversable(randomLocation))
                    {
                        return randomLocation;
                    }
                }
            }
        }

        public IEnumerable<Pellet> VisiblePelletsFrom(Location location)
        {
            var sightMask = GenerateSightMask(location);
            for (short row = 0; row < sightMask.Length; row++)
            {
                ulong visibleSmallInRow = sightMask[row] & _visibleSmallPellets[row];
                ulong visibleBigInRow = sightMask[row] & _bigPellets[row];

                for (short cell = 0; cell < Width; cell++)
                {
                    var binaryValue = (ulong) Math.Pow(2, Width - cell - 1);
                    if ((binaryValue & visibleSmallInRow) == binaryValue)
                    {
                        yield return new Pellet(new Location(cell, row), 1);
                    }
                    else if ((binaryValue & visibleBigInRow) == binaryValue)
                    {
                        yield return new Pellet(new Location(cell, row), 10);
                    }
                }
            }
        }

        /// <summary>
        /// 1 means a cell is visible. 0 Means it is not.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private ulong[] GenerateSightMask(Location location)
        {
            ulong[] sightMask = new ulong[Height];

            Func<Location, Location>[] directions = {North, South, East, West};

            foreach (var direction in directions)
            {
                var newLocation = direction(location);
                while (Traversable(newLocation))
                {
                    sightMask[newLocation.GridRow] = newLocation.GridMask(Width) | sightMask[newLocation.GridRow];
                    newLocation = direction(newLocation);
                }
            }

            return sightMask;
        }

        public void ClearEmptyPelletsVisibleFromLocation(Location location)
        {
            var sightMask = GenerateSightMask(location);
            var remainderMaskArray = new ulong[Height];
            for (int row = 0; row < Height; row++)
            {
                // Set any visible area to 0 in the remainder mask
                var remainderMask = (ulong.MaxValue ^ sightMask[row]) & _possibleSmallPellets[row];
                remainderMaskArray[row] = remainderMask;
                // If the pellet is outside the sightMask or is visible, leave it to be 1
                _possibleSmallPellets[row] = remainderMask | (_visibleSmallPellets[row] & sightMask[row]);
            }
        }

        public IEnumerable<Pac> VisibleEnemiesFrom(Location location)
        {
            throw new NotImplementedException();
            //_cells[location].PossiblePelletValue = 0;
            //return VisibleTFrom(location, _enemies);
        }

        public Location West(Location origin)
        {
            var newX = (short)(origin.X - 1);
            if (newX < 0)
            {
                newX = (short)(Width - 1);
            }

            return new Location(newX, origin.Y);
        }

        public Location East(Location origin)
        {
            var newX = (short)(origin.X + 1);
            if (newX >= Width)
            {
                newX = (short)(0);
            }

            return new Location(newX, origin.Y);
        }
        public Location South(Location origin)
        {
            var newY = (short)(origin.Y + 1);
            if (newY >= Height)
            {
                newY = (short)(0);
            }

            return new Location(origin.X, newY);
        }
        public Location North(Location origin)
        {
            var newY = (short)(origin.Y - 1);
            if (newY < 0)
            {
                newY = (short)(Height - 1);
            }

            return new Location(origin.X, newY);
        }

        public short NeighbourCount(Location origin)
        {
            short Func(Location c, Func<Location, Location> f) => (short) (Traversable(f(c)) ? 1 : 0);

            var edges = new Func<Location, Location>[] { North, South, East, West };

            return (short)edges.Sum(e => Func(origin, e));
        }

        private IEnumerable<T> VisibleTFrom<T>(Location location, IDictionary<Location, T> dictionary)
        {
            throw new NotImplementedException();

            //Func<Location, GridCell>[] searches =
            //{
            //    North,
            //    South,
            //    East,
            //    West,
            //};

            //List<Location> visited = new List<Location>();

            //foreach (var search in searches)
            //{
            //    var next = search(location);
            //    while (next.Traversable && !visited.Contains(next.Location))
            //    {
            //        visited.Add(next.Location);

            //        // Clean up the pellets if none are visible
            //        if (next.PossiblePelletValue > 0 && !_visibleSmallPellets.ContainsKey(next.Location))
            //        {
            //            next.PossiblePelletValue = 0;
            //        }

            //        if (dictionary.ContainsKey(next.Location))
            //        {
            //            yield return dictionary[next.Location];
            //        }

            //        next = search(next.Location);
            //    }
            //}
        }

        public bool Traversable(Location location)
            => (_grid[location.GridRow] & location.GridMask(Width)) == location.GridMask(Width);

        public void ClearPossiblePelletAt(Location pacLocation)
        {
            var row = pacLocation.GridRow;
            var invertedMask = ulong.MaxValue ^ pacLocation.GridMask(Width);
            _possibleSmallPellets[row] = _possibleSmallPellets[row] & invertedMask;
        }
    }
}