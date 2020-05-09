namespace pacman
{
    using System;
    using System.Threading;
    using System.Collections.Generic;

    public class ClosestFoodMovementStrategy : IMovementStrategy
    {
        private const int MaxRadius = 10;
        private readonly Dictionary<int, NextMove> _nextMove = new Dictionary<int, NextMove>();

        public NextMove Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            if (_nextMove.ContainsKey(pac.Id) && gameGrid.FoodValue(_nextMove[pac.Id].Location) > 0)
            {
                return _nextMove[pac.Id];
            }

            if (_nextMove.ContainsKey(pac.Id) && gameGrid.FoodValue(_nextMove[pac.Id].Location) == 0)
            {
                _nextMove.Remove(pac.Id);
            }

            int searchRadius = 1;
            var pacX = pac.Location.X;
            int searchX = pacX - searchRadius;
            var pacY = pac.Location.Y;
            int searchY = pacY - searchRadius;

            while (searchRadius != MaxRadius + 1
                   || !cancellation.IsCancellationRequested)
            {
                Location location = new Location(searchX, searchY);

                if (searchX == pacX + searchRadius
                    && searchY == pacY + searchRadius)
                {
                    searchRadius++;
                    searchX = pacX - searchRadius;
                    searchY = pacY - searchRadius;
                }
                else if (searchX == pacX + searchRadius)
                {
                    searchX = pacX - searchRadius;
                    searchY++;
                }
                else
                {
                    searchX++;
                }

                if (!gameGrid.InBounds(location))
                {
                    continue;
                }

                if (gameGrid.FoodValue(location) > 0)
                {
                    Console.Error.WriteLine($"Radius {searchRadius} From {pac.Location} food {location} Mine {pac.Mine}");
                    _nextMove[pac.Id] = new NextMove(pac, location);

                    return _nextMove[pac.Id];
                }
            }

            if (cancellation.IsCancellationRequested)
            {
                Console.Error.WriteLine("Aborting move search due to cancellation");
                return new NextMove(pac, pac.Location);
            }

            Console.Error.WriteLine("No pellets found in range");
            return new NextMove(pac, pac.Location);
        }
    }
}