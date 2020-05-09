namespace pacman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class GameGrid
    {
        private readonly Dictionary<Location, GridCell> _cells = new Dictionary<Location, GridCell>();
        private Dictionary<Location, Pellet> _pellets = new Dictionary<Location, Pellet>();

        public GridCell this[Location location] => _cells[location];

        public void StoreGrid(IEnumerable<string> gridLines)
        {
            int row = 0;
            foreach (string line in gridLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                for (int cell = 0; cell < line.Length; cell++)
                {
                    var location = new Location(cell, row);
                    try
                    {
                        switch (line[cell])
                        {
                            case '#':
                                _cells.Add(location, new GridCell(false));
                                continue;
                            case ' ':
                                _cells.Add(location, new GridCell(true));
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
            GridCell original = _cells[location];
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

        public struct GridCell
        {
            public GridCell(bool traversable
            ) : this(traversable, null)
            {
            }

            public GridCell(bool traversable,
                Pac? pac
            )
            {
                Traversable = traversable;
                Pac = pac;
            }

            public bool Traversable { get; }
            public Pac? Pac { get; }
            public bool HasPac => Pac.HasValue;
        }
    }
}