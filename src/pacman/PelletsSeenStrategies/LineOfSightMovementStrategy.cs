namespace pacman.PelletsSeenStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    public class LineOfSightMovementStrategy : IPelletsSeenStrategy
    {
        private static readonly Random Random = new Random();

        public NextAction Next(GameGrid gameGrid, 
            Pac pac, 
            CancellationToken cancellation,
            List<Pellet> visiblePellets)
        {
            Location loc = pac.Location;
            Location targetLocation;

            if (!visiblePellets.Any())
            {
                if (pac.LastMoveAction.Location != pac.Location)
                {
                    return new MoveAction(pac, pac.LastMoveAction.Location);
                }

                targetLocation = gameGrid.RandomLocation;
                Console.Error.WriteLine($"Pac {pac.Id} No food. Going to random location {targetLocation}");
            }
            else
            {
                var closestY = visiblePellets.OrderBy(p => Math.Abs(p.Location.X - loc.X)).FirstOrDefault();
                var closestX = visiblePellets.OrderBy(p => Math.Abs(p.Location.Y - loc.Y)).FirstOrDefault();

                if (Math.Abs(closestX.Location.X - loc.X) < Math.Abs(closestY.Location.Y - loc.Y))
                {
                    targetLocation = closestX.Location;
                }
                else
                {
                    targetLocation = closestX.Location;
                }

                if (pac.SpecialActionReady && Random.Next(0, 9) <= 2)
                {
                    return new SpeedAction(pac);
                }

                Console.Error.WriteLine($"Pac {pac.Id} Found food. Going to location {targetLocation}");
            }

            return new MoveAction(pac, targetLocation);
        }
    }
}