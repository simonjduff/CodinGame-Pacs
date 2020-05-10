namespace pacman.ActionStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    [Obsolete("This isn't useful when food is invisible")]
    public class ClosestFoodMovementStrategy : IActionStrategy
    {
        private const int MaxRadius = 10;
        private readonly Dictionary<PacKey, MoveAction> _nextMove = new Dictionary<PacKey, MoveAction>();

        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            if (_nextMove.ContainsKey(pac.Key) && gameGrid.FoodValue(_nextMove[pac.Key].Location) > 0)
            {
                return _nextMove[pac.Key];
            }

            if (_nextMove.ContainsKey(pac.Key) && gameGrid.FoodValue(_nextMove[pac.Key].Location) == 0)
            {
                _nextMove.Remove(pac.Key);
            }

            short searchRadius = 1;
            short pacX = pac.Location.X;
            short searchX = (short)(pacX - searchRadius);
            short pacY = pac.Location.Y;
            short searchY = (short)(pacY - searchRadius);

            while (searchRadius != MaxRadius + 1
                   || !cancellation.IsCancellationRequested)
            {
                Location location = new Location(searchX, searchY);

                if (searchX == pacX + searchRadius
                    && searchY == pacY + searchRadius)
                {
                    searchRadius++;
                    searchX = (short)(pacX - searchRadius);
                    searchY = (short)(pacY - searchRadius);
                }
                else if (searchX == pacX + searchRadius)
                {
                    searchX = (short)(pacX - searchRadius);
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
                    _nextMove[pac.Key] = new MoveAction(pac, location);

                    return _nextMove[pac.Key];
                }
            }

            if (cancellation.IsCancellationRequested)
            {
                Console.Error.WriteLine("Aborting move search due to cancellation");
                return new MoveAction(pac, pac.Location);
            }

            Console.Error.WriteLine("No pellets found in range");
            return new MoveAction(pac, pac.Location);
        }
    }
}