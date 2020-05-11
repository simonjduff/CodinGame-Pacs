namespace pacman.ActionStrategies
{
    using System;
    using System.Threading;
    [Obsolete("This isn't useful when food is invisible")]
    public class ClosestFoodMovementStrategy : IActionStrategy
    {
        private readonly GameGrid _gameGrid;
        private const int MaxRadius = 10;

        public ClosestFoodMovementStrategy(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }

        public NextAction Next(Pac pac, CancellationToken cancellation)
        {
            if (pac.LastMoveAction != null && _gameGrid.FoodValue(pac.LastMoveAction.Location) > 0)
            {
                return new NoAction(pac);
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

                if (!_gameGrid.InBounds(location))
                {
                    continue;
                }

                if (_gameGrid.FoodValue(location) > 0)
                {
                    Console.Error.WriteLine($"Radius {searchRadius} From {pac.Location} food {location} Mine {pac.Mine}");

                    return new MoveAction(pac, location);
                }
            }

            if (cancellation.IsCancellationRequested)
            {
                Console.Error.WriteLine("Aborting move search due to cancellation");
                return new MoveAction(pac, pac.Location);
            }

            Console.Error.WriteLine("No pellets found in range");
            return new NoAction(pac);
        }
    }
}