namespace pacman.ActionStrategies
{
    using System;
    using System.Linq;
    using System.Threading;
    public class LineOfSightMovementStrategy : IActionStrategy
    {
        private static readonly Random Random = new Random();

        public NextAction Next(GameGrid gameGrid, Pac pac, CancellationToken cancellation)
        {
            Location loc = pac.Location;
            Location targetLocation;

            var visiblePellets = gameGrid.VisiblePelletsFrom(pac.Location).ToArray();

            if (!visiblePellets.Any())
            {
                targetLocation = new Location((short)Random.Next(0, gameGrid.Width), (short)Random.Next(0, gameGrid.Height));
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
            }

            if (pac.LastMoveAction?.Location == targetLocation)
            {
                return new NoAction(pac);
            }

            return new MoveAction(pac, targetLocation);
        }
    }
}