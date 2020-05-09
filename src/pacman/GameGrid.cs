﻿namespace pacman
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
                Pac pac
            )
            {
                Traversable = traversable;
                Pac = pac;
            }

            public bool Traversable { get; }
            public Pac Pac { get; }
        }

        /// <summary>
        /// This can contain duplicates if the level wraps around.
        /// Look left, see yourself, look right, see yourself. Double count food.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEnumerable<Pellet> VisiblePelletsFrom(Location location)
        {
            foreach (var result in (SearchNDirection(location, 
                l => l.X < 0 ? new Location(Width, l.Y) : new Location((short)(l.X - 1), l.Y))))
            {
                yield return result;
            }
            foreach (var result in (SearchNDirection(location, 
                l => l.X >= Width -1 ? new Location(0, l.Y) : new Location((short)(l.X + 1), l.Y))))
            {
                yield return result;
            }
            foreach (var result in (SearchNDirection(location, 
                l => l.Y < 0 ? new Location(l.X, Height) : new Location(l.X, (short)(l.Y - 1)))))
            {
                yield return result;
            }
            foreach (var result in (SearchNDirection(location, 
                l => l.Y >= Height -1 ? new Location(l.X, 0) : new Location(l.X, (short)(l.Y + 1)))))
            {
                yield return result;
            }
        }

        private IEnumerable<Pellet> SearchNDirection(Location location, 
            Func<Location, Location> next)
        {
            var search = next(location);
            do
            {
                if (!_cells.ContainsKey(search))
                {
                    Console.Error.WriteLine($"Tried to search invalid cell {search}");
                    break;
                }

                var cell = _cells[search];
                if (!cell.Traversable)
                {
                    break;
                }

                if (_pellets.ContainsKey(search))
                {
                    yield return _pellets[search];
                }

                if (search.Equals(location))
                {
                    // We wrapped around
                    break;
                }

                search = next(search);
            } while (true);
        }
    }
}